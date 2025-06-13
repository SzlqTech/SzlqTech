using NLog;
using SzlqTech.Common.Extensions;
using SzlqTech.Common.MultiThreads;
using SzlqTech.Common.Nlogs;
using SzlqTech.Equipment;
using SzlqTech.Equipment.Machine;
using SzlqTech.Equipment.Scanner;

namespace SzlqTech.Core.WorkFlow
{
    public class BaseWorkFlow : IWorkCore
    {
        public IExecutingMachine ExecutingMachine { get; }
        public IExecutingScanner ExecutingScanner { get; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 数据库更新锁
        /// </summary>
        protected virtual SemaphoreQueue UpdateLock { get; set; } = new SemaphoreQueue(1, 1);

        public BaseWorkFlow(IExecutingMachine ExecutingMachine, IExecutingScanner ExecutingScanner)
        {
            this.ExecutingMachine = ExecutingMachine;
            this.ExecutingScanner = ExecutingScanner;
        }

        public bool StartExecute()
        {
            bool result = false;
            try
            {
                ExecutingScanner.GetScanners();
                ExecutingMachine.GetMachines();
                StartScanners();
                StartMachine();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                logger.ErrorHandler(ex, $"程序异常即将停止, {ex.Message}");
                StopExecute();
                return  false;
            }
            finally
            {
                 
            }
        }

        public bool StopExecute()
        {
            bool result = false;
            try
            {
                
                //停止IO设备
                StopMachine();
                //等待数据处理完成
                UpdateLock.Wait();
                //停止扫描设备
                StopScanners();
                result= true;
                return result;

            }
            catch (Exception ex)
            {
                logger.ErrorHandler(ex, "停止发生异常,原因：" + ex.Message);
                return  false;
            }
            finally
            {
                
            }
        }

        #region 机器启动
        /// <summary>
        /// 启动机器
        /// </summary>
        public virtual void StartMachine()
        {
            ExecutingMachine.OpenMachine();
            ExecutingMachine.RegisterMachineEvent();
            ExecutingMachine.DataReceived -= ExecutingMachine_DataReceived;
            ExecutingMachine.DataReceived += ExecutingMachine_DataReceived;
            ExecutingMachine.StartMachine();
        }

        /// <summary>
        /// 停止机器
        /// </summary>
        public virtual void StopMachine()
        {
            ExecutingMachine.StopMachine();
            ExecutingMachine.UnRegisterMachineEvent();
            ExecutingMachine.DataReceived -= ExecutingMachine_DataReceived;
            ExecutingMachine.CloseMachine();
        }

        /// <summary>
        /// 收到设备数据时处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public  void ExecutingMachine_DataReceived(object? sender, TEventArgs<MachineData> e)
        {
            OnExecutingMachineDataReceived(e.Data);
        }


        /// <summary>
        /// 可重写的设备数据接收处理方法
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnExecutingMachineDataReceived(MachineData data)
        {

        }
        #endregion

        #region 扫描设备
        /// <summary>
        /// 按配置获取打印器
        /// </summary>
        protected virtual void StartScanners()
        {
            foreach (IScanner scanner in ExecutingScanner.ScannerList)
            {
                scanner.DataReceived -= Scanner_DataReceived;
                scanner.DataReceived += Scanner_DataReceived;
                scanner.Open();
            }
        }


        /// <summary>
        /// 关闭扫描设备并取消事件订阅
        /// </summary>
        protected virtual void StopScanners()
        {
            foreach (IScanner scanner in ExecutingScanner.ScannerList.Where(scanner => scanner != null))
            {
                if (scanner.IsOpen)
                {
                    scanner.Close();
                }
                scanner.DataReceived -= Scanner_DataReceived;
            }
        }

        /// <summary>
        /// 扫描触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scanner_DataReceived(object? sender, TEventArgs<ScanData> e)
        {
            try
            {
                // Thread.CurrentThread.Priority = ThreadPriority.Highest;
                OnScannerDataReceivedAsync(e.Data);
            }
            catch (Exception exception)
            {
                logger.ErrorHandler(exception, $"包装扫描数据处理异常, 即将停止, 消息: [{e.Data.Data}] {exception.Message}");
                StopExecute();
            }
        }

        /// <summary>
        /// 收到扫描数据处理
        /// </summary>
        /// <param name="scanData"></param>
        /// <returns></returns>
        protected virtual void OnScannerDataReceivedAsync(ScanData scanData)
        {
            //switch (scanData.CodeLevel)
            //{
            //    ba
            //}
        } 
        #endregion

        public async Task WaitDataActionAsync(Action action,string? Message=null)
        {
            try
            {
                await Task.Run(action);
            }
            catch (Exception ex)
            {
                logger.ErrorHandler($"系统等待异常:{ex.Message}");
            }
            finally
            {

            }
        }

        public async Task<bool> WaitDataActionResultAsync(Func<bool> action, string? Message = null)
        {
            try
            {
                var res=  await Task.Run(action);
                return res;
            }
            catch (Exception ex)
            {
                logger.ErrorHandler($"系统等待异常:{ex.Message}");
                return false;
            }      
        }

        #region 多任务加锁

        /// <summary>
        /// 数据库多线程任务操作加锁
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual async Task WaitUpdateLockAsync(Func<Task> task)
        {
            await UpdateLock.WaitAsync();
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                logger.ErrorHandler($"更新锁异常:{ex.Message}");
            }
            finally
            {
                UpdateLock.Release();
            }
        }

        /// <summary>
        /// 数据库多线程任务操作加锁
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual async Task WaitUpdateActionAsync(Action action)
        {
            await UpdateLock.WaitAsync();
            try
            {
                await Task.Run(action);
            }
            catch (Exception ex)
            {
                logger.ErrorHandler($"更新锁异常:{ex.Message}");
            }
            finally
            {
                UpdateLock.Release();
            }
        }

        #endregion


    }
}
