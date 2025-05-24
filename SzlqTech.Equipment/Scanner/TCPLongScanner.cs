using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Timers;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Extensions;
using NLog;
using Timer = System.Timers.Timer;
using SzlqTech.Common.Nlogs;

namespace SzlqTech.Equipment.Scanner
{
    public abstract class TCPLongScanner : IScanner//, BaseTCPClient
    {
        private readonly IPEndPoint remotePoint;

        private Socket? clientSocketToService;

        private readonly byte[] result = new byte[1024];

        private readonly int _codeLevel;

        private readonly int keepAlive = -1744830460; //?SIO_KEEPALIVE_VALS

        private readonly byte[] inValue = { 1, 0, 0, 0, 0xa0, 0x0f, 0, 0, 0xd0, 0x07, 0, 0 }; //True, 20 秒, 2 秒

        private const int reconnectInterval = 10;

        private readonly Timer reconnectTimer = new Timer(reconnectInterval);

        private readonly int maxReconnectTime = 5;

        private int currentReconnectTime = 0;

        private bool connectedFlag = false;

        // private const int checkConnectInterval = 60 * 60 * 1000;
        // private readonly Timer checkConnectTimer = new Timer(checkConnectInterval);

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        protected TCPLongScanner(string portName, string portKey, int codeLevel)
        {
            string[] ls = Regex.Split(portKey, ":", RegexOptions.IgnoreCase);
            try
            {
                remotePoint = new IPEndPoint(IPAddress.Parse(ls[0]), int.Parse(ls[1]));
                this.PortName = portName;
                this._codeLevel = codeLevel;
                this.PortKey = portKey;
            }
            catch (Exception ex)
            {
                // LogHelper.Error(ex);
                throw new Exception($"端口[{portName}]通讯地址[{portKey}]格式不正确", ex);
            }
        }


        public bool IsOpen { get; set; } = false;

        public string PortKey { get; }

        public string PortName { get; }

        public int CodeLevel
        {
            get { return _codeLevel; }
        }

        public event EventHandler<TEventArgs<ScanData>>? DataReceived;

        /// <summary>
        /// 打开读码器连接
        /// </summary>
        public virtual void Open()
        {
            try
            {
                Connect();
                IsOpen = true;
            }
            catch (Exception ex)
            {
                clientSocketToService?.Close();
                throw new Exception($"连接采集设备[{PortName}]{PortKey}失败, 异常消息: {ex.Message}", ex);
            }


        }

        public virtual void Close()
        {
            DisableReconnectTimer();
            if (clientSocketToService != null && clientSocketToService.Connected)
            {
                clientSocketToService.Disconnect(false);
            }
            if (clientSocketToService != null)
            {
                try
                {
                    Thread.Sleep(200);
                    clientSocketToService?.Close();
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
            IsOpen = false;
        }

        private void Connect()
        {
            if (clientSocketToService == null || clientSocketToService.Connected == false)
            {
                clientSocketToService = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocketToService.IOControl(keepAlive, inValue, null);
                clientSocketToService.Connect(remotePoint);
            }
            if (clientSocketToService.Connected == false)
            {
                // clientSocketToService.IOControl(keepAlive, inValue, null);
                clientSocketToService.Connect(remotePoint);
            }
            SocketReceiveData socketReceiveDatum = new SocketReceiveData()
            {
                ReceiveSocket = clientSocketToService
            };
            clientSocketToService.BeginReceive(socketReceiveDatum.ReceiveBuffer, 0, socketReceiveDatum.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataRead), socketReceiveDatum);
        }

        /// <summary>
        /// 数据读取后发生
        /// </summary>
        /// <param name="ar"></param>
        private void OnDataRead(IAsyncResult ar)
        {
            try
            {
                if (ar.AsyncState is SocketReceiveData asyncState)
                {
                    if (clientSocketToService != null && clientSocketToService.Connected)
                    {
                        int num = asyncState.ReceiveSocket!.EndReceive(ar, out SocketError socketError);
                        if (socketError != SocketError.Success)
                        {
                            string? errorMessage = GetErrorMessage(socketError);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                throw new Exception(errorMessage);
                            }
                            // LogHelper.ErrorHandler(errorMessage);
                            // asyncState.ReciveSocket.BeginReceive(asyncState.ReciveBuffer, 0, asyncState.ReciveBuffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), asyncState);

                        }
                        else if (num != 0)
                        {
                            asyncState.MemoryStream.Write(asyncState.ReceiveBuffer, 0, num);
                            if (asyncState.ReceiveSocket.Available <= 0)
                            {
                                byte[] array = asyncState.MemoryStream.ToArray();
                                asyncState.MemoryStream.Close();
                                if (IsOpen)
                                {
                                    //字节
                                    ProcessData(array);
                                }
                                SocketReceiveData socketReceiveDatum = new SocketReceiveData()
                                {
                                    ReceiveSocket = asyncState.ReceiveSocket
                                };
                                socketReceiveDatum.ReceiveSocket.BeginReceive(socketReceiveDatum.ReceiveBuffer, 0, socketReceiveDatum.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataRead), socketReceiveDatum);
                            }
                            else
                            {
                                asyncState.ReceiveSocket.BeginReceive(asyncState.ReceiveBuffer, 0, asyncState.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(OnDataRead), asyncState);
                            }
                        }
                        else
                        {
                            connectedFlag = false;
                            if (IsOpen)
                            {
                                Close();
                                EnableReconnectTimer();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorHandler($"采集器网口发生故障, 系统尝试自动重连, 故障原因: {ex.Message}");
                connectedFlag = false;
                clientSocketToService?.Disconnect(false);
                EnableReconnectTimer();
                // if (IsOpen)
                // {
                //
                //     reconnectTimer.Enabled = true;
                //     LogHelper.ErrorHandler(ex);
                //     throw new Exception(ex.Message, ex);
                // }
            }
        }

        /// <summary>
        /// 引发数据接收事件
        /// </summary>
        /// <param name="scanData"></param>
        protected void RaiseDataReceived(ScanData scanData)
        {
            DataReceived?.Invoke(this, new TEventArgs<ScanData>(scanData));
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="bytes"></param>
        protected virtual void ProcessData(byte[] bytes)
        {

        }

        /// <summary>
        /// 获取错误消息
        /// </summary>
        /// <param name="socketError"></param>
        /// <returns></returns>
        protected string? GetErrorMessage(SocketError socketError)
        {
            string? errorMessage = null;
            switch (socketError)
            {
                case SocketError.Success:
                    break;
                case SocketError.SocketError:
                    errorMessage = "发生了未指定的 Socket 错误";
                    break;
                case SocketError.OperationAborted:
                    errorMessage = "由于 Socket 已关闭，重叠的操作被中止";
                    break;
                case SocketError.IOPending:
                    errorMessage = "应用程序已启动一个无法立即完成的重叠操作";
                    break;
                case SocketError.Interrupted:
                    errorMessage = "已取消阻止 Socket 调用的操作";
                    break;
                case SocketError.AccessDenied:
                    errorMessage = "已试图通过被其访问权限禁止的方式访问 Socket";
                    break;
                case SocketError.Fault:
                    errorMessage = " 基础套接字提供程序检测到无效的指针地址";
                    break;
                case SocketError.InvalidArgument:
                    errorMessage = "给 Socket 成员提供了一个无效参数";
                    break;
                case SocketError.TooManyOpenSockets:
                    errorMessage = "基础套接字提供程序中打开的套接字太多";
                    break;
                case SocketError.WouldBlock:
                    errorMessage = "对非阻止性套接字的操作不能立即完成";
                    break;
                case SocketError.InProgress:
                    errorMessage = "阻止操作正在进行中";
                    break;
                case SocketError.AlreadyInProgress:
                    errorMessage = "非阻止性 Socket 已有一个操作正在进行中";
                    break;
                case SocketError.NotSocket:
                    errorMessage = "对非套接字尝试 Socket 操作";
                    break;
                case SocketError.DestinationAddressRequired:
                    errorMessage = "在对 Socket 的操作中省略了必需的地址";
                    break;
                case SocketError.MessageSize:
                    errorMessage = "数据报太长";
                    break;
                case SocketError.ProtocolType:
                    errorMessage = "此 Socket 的协议类型不正确";
                    break;
                case SocketError.ProtocolOption:
                    errorMessage = "对 Socket 使用了未知、无效或不受支持的选项或级别";
                    break;
                case SocketError.ProtocolNotSupported:
                    errorMessage = "未实现或未配置协议";
                    break;
                case SocketError.SocketNotSupported:
                    errorMessage = "在此地址族中不存在对指定的套接字类型的支持";
                    break;
                case SocketError.OperationNotSupported:
                    errorMessage = "协议族不支持地址族";
                    break;
                case SocketError.ProtocolFamilyNotSupported:
                    errorMessage = "未实现或未配置协议族";
                    break;
                case SocketError.AddressFamilyNotSupported:
                    errorMessage = "地址与请求的协议不兼容";
                    break;
                case SocketError.AddressAlreadyInUse:
                    errorMessage = "通常，只允许使用地址一次";
                    break;
                case SocketError.AddressNotAvailable:
                    errorMessage = "选定的地址在此上下文中有效";
                    break;
                case SocketError.NetworkDown:
                    errorMessage = "网络不可用";
                    break;
                case SocketError.NetworkUnreachable:
                    errorMessage = "不存在路由或远程主机不可达";
                    break;
                case SocketError.NetworkReset:
                    errorMessage = "应用程序试图在已超时的连接上设置 KeepAlive";
                    break;
                case SocketError.ConnectionAborted:
                    errorMessage = "此连接由 .NET Framework 或基础套接字提供程序中止";
                    break;
                case SocketError.ConnectionReset:
                    errorMessage = "此连接由远程对等计算机重置";
                    break;
                case SocketError.NoBufferSpaceAvailable:
                    errorMessage = "没有可用于 Socket 操作的可用缓冲区空间";
                    break;
                case SocketError.IsConnected:
                    errorMessage = "Socket 已连接";
                    break;
                case SocketError.NotConnected:
                    errorMessage = "应用程序试图发送或接收数据，但是 Socket 未连接";
                    break;
                case SocketError.Shutdown:
                    errorMessage = "发送或接收数据的请求未得到允许，因为 Socket 已被关闭";
                    break;
                case SocketError.TimedOut:
                    errorMessage = "连接尝试超时，或者连接的主机没有响应";
                    break;
                case SocketError.ConnectionRefused:
                    errorMessage = "远程主机正在主动拒绝连接";
                    break;
                case SocketError.HostDown:
                    errorMessage = "由于远程主机被关闭，操作失败";
                    break;
                case SocketError.HostUnreachable:
                    errorMessage = "没有到指定主机的网络路由";
                    break;
                case SocketError.ProcessLimit:
                    errorMessage = "正在使用基础套接字提供程序的进程过多";
                    break;
                case SocketError.SystemNotReady:
                    errorMessage = "网络子系统不可用";
                    break;
                case SocketError.VersionNotSupported:
                    errorMessage = "基础套接字提供程序的版本超出范围";
                    break;
                case SocketError.NotInitialized:
                    errorMessage = "尚未初始化基础套接字提供程序";
                    break;
                case SocketError.Disconnecting:
                    errorMessage = "正常关机正在进行中";
                    break;
                case SocketError.TypeNotFound:
                    errorMessage = "未找到指定的类";
                    break;
                case SocketError.HostNotFound:
                    errorMessage = "无法识别这种主机。该名称不是正式的主机名或别名";
                    break;
                case SocketError.TryAgain:
                    errorMessage = "无法解析主机名。请稍后重试";
                    break;
                case SocketError.NoRecovery:
                    errorMessage = "错误不可恢复或找不到请求的数据库";
                    break;
                case SocketError.NoData:
                    errorMessage = "在名称服务器上找不到请求的名称或 IP 地址";
                    break;
            }
            return errorMessage;
        }

        /// <summary>
        /// 使能重连定时器
        /// </summary>
        protected void EnableReconnectTimer()
        {
            reconnectTimer.Elapsed += ReconnectTimer_Elapsed;
            reconnectTimer.Interval = reconnectInterval;
            reconnectTimer.Enabled = true;
        }

        /// <summary>
        /// 禁用重连定时器
        /// </summary>
        protected void DisableReconnectTimer()
        {
            reconnectTimer.Elapsed -= ReconnectTimer_Elapsed;
            reconnectTimer.Enabled = false;
        }

        /// <summary>
        /// 重连定时器到时间处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // LogHelper.DebugHandler("ReconnectTimer_Elapsed " + currentReconnectTime);
            if (!connectedFlag) Reconnect();
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        protected void Reconnect()
        {
            // checkConnectTimer.Enabled = false;
            currentReconnectTime += 1;
            // LogHelper.DebugHandler("Reconnect  " + currentReconnectTime);
            if (currentReconnectTime >= maxReconnectTime)
            {
                DisableReconnectTimer();
                throw new EquipmentException($"扫描器{PortName}失去连接，尝试重连已达到最大重试次数{maxReconnectTime}，请检查通讯是否正常，然后重启软件");
                // reconnectTimer.Enabled = false;
                // checkConnectTimer.Enabled = false;
                // return;
            }
            logger.ErrorHandler($"扫描器{PortName}去连接,系统正在进行第{currentReconnectTime}次重连");
            if (clientSocketToService == null || !clientSocketToService.Connected)
            {
                Connect();
            }
            if (clientSocketToService != null && clientSocketToService.Connected)
            {
                DisableReconnectTimer();
                connectedFlag = true;
                logger.InfoHandler($"扫描器重连成功");
                currentReconnectTime = 0;
            }
        }
    }

    /// <summary>
    /// Socket接收数据
    /// </summary>
    public class SocketReceiveData
    {
        /// <summary>
        /// 缓冲大小
        /// </summary>
        private const int bufferSize = 20480;

        /// <summary>
        /// 接收缓冲
        /// </summary>
        public byte[] ReceiveBuffer = new byte[bufferSize];

        /// <summary>
        /// 接收Socket
        /// </summary>
        public Socket? ReceiveSocket;

        /// <summary>
        /// 接收流
        /// </summary>
        public MemoryStream MemoryStream = new MemoryStream();

        /// <summary>
        /// 
        /// </summary>
        public TcpClient? NetWork;

        /// <summary>
        /// 
        /// </summary>
        public string? Name;
    }
}
