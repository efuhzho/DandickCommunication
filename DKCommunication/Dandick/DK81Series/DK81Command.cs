using DKCommunication. Dandick. Base;
using System;
using DKCommunication. BasicFramework;
using DKCommunication. Core;

namespace DKCommunication. Dandick. DK81Series
{
    /// <summary>
    /// 丹迪克81协议的命令格式，可以携带站号（ID）、命令码（CommandCode）、数据（DATA）
    /// </summary>
    public class DK81Command : DK_DeviceBase<RegularByteTransform>
    {
        #region --------------------------------- Constructor -------------------------------------
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）
        /// </summary>
        public DK81Command ( )
        {
            var result = AnalysisID ( 0 );
            if ( result. IsSuccess )
            {
                _RxID = result. Content[1];
                _TxID = result. Content[0];
            }
            else
            {
                throw new Exception ( result. Message );
            }
        }

        /// <summary>
        /// 实例化一个指定ID的对象
        /// </summary>
        /// <param name="id">读取的终端ID</param>
        public DK81Command ( ushort id )
        {
            var result = AnalysisID ( id );
            if ( result. IsSuccess )
            {
                _RxID = result. Content[1];
                _TxID = result. Content[0];
            }
            else
            {
                throw new Exception ( result. Message );
            }
        }
        #endregion Constructor

        #region --------------------------------- 私有字段 ----------------------------------------
        #region ID
        /// <summary>
        /// 接收终端的设备ID
        /// </summary>
        private readonly byte _RxID;

        /// <summary>
        /// 发送终端的设备ID
        /// </summary>
        private readonly byte _TxID;



        #endregion

        /*******************/

        #region Ranges
        //private byte RangeUa;
        //private byte RangeUb;
        //private byte RangeUc;
        //private byte RangeIa;
        //private byte RangeIb;
        //private byte RangeIc;
        //private byte RangeIPa;
        //private byte RangeIPb;
        //private byte RangeIPc;
        #endregion Ranges

        #endregion 私有字段

        #region --------------------------------- Public Properties--------------------------------

        #endregion Public Properties

        #region --------------------------------- Private CommandBuilder Helper 【报文创建助手】---
        /// <summary>
        /// 创建完整指令长度的【指令头】，长度大于7的报文不带CRC校验码，不可直接发送给串口，长度等于7则带校验码
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">指令长度</param>
        /// <returns>带指令信息的结果：完整指令长度</returns>
        private OperateResult<byte[]> CreateCommandHelper ( byte commandCode , ushort commandLength )
        {
            //尝试预创建报文
            try
            {
                byte[] buffer = new byte[commandLength];
                buffer[0] = DK81CommunicationInfo. FrameID;
                buffer[1] = _RxID;
                buffer[2] = _TxID;
                buffer[3] = BitConverter. GetBytes ( commandLength )[0];
                buffer[4] = BitConverter. GetBytes ( commandLength )[1];
                buffer[5] = commandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake 
                if ( commandLength == 7 )
                {
                    buffer[6] = DK81CommunicationInfo. CRCcalculator ( buffer );    //如果是不带数据的命令则加上校验码
                }
                return OperateResult. CreateSuccessResult ( buffer );
            }

            //发生异常
            catch ( Exception ex )
            {
                return new OperateResult<byte[]> ( 811200 , ex. Message + "From:CreateCommandHelper" );
            }
        }

        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <typeparam name="T">泛型枚举，必须可以被转换为byte</typeparam>
        /// <param name="data">数据</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateCommandHelper<T> ( byte commandCode , ushort commandLength , T byteData ) where T : Enum //TODO 添加T类型约束
        {
            try
            {
                OperateResult<byte[]> result = CreateCommandHelper ( commandCode , commandLength );

                if ( result. IsSuccess )
                {
                    result. Content[6] = Convert. ToByte ( byteData );
                    result. Content[7] = DK81CommunicationInfo. CRCcalculator ( result. Content );
                    return result;
                }
                else
                {
                    return result;
                }
            }
            catch ( Exception ex )
            {
                return new OperateResult<byte[]> ( 811202 , ex. Message + "From:CreateCommandHelper" );
            }
        }

        /// <summary>
        /// 创建完整的指令，带CRC校验码，可直接发送给串口
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">完整指令长度</param>
        /// <param name="dataBytes">传入的数据</param>
        /// <returns>完整的指令，带CRC校验码，可直接发送给串口</returns>
        private OperateResult<byte[]> CreateCommandHelper ( byte commandCode , ushort commandLength , byte[] dataBytes )
        {
            try
            {
                OperateResult<byte[]> dataBytesLess = CreateCommandHelper ( commandCode , commandLength );
                if ( dataBytesLess. IsSuccess )
                {
                    Array. Copy ( dataBytes , 0 , dataBytesLess. Content , 6 , dataBytes. Length );
                    dataBytesLess. Content[commandLength - 1] = DK81CommunicationInfo. CRCcalculator ( dataBytesLess. Content );
                    return dataBytesLess;
                }
                else
                {
                    return dataBytesLess;
                }
            }
            catch ( Exception ex )
            {
                return new OperateResult<byte[]> ( 811204 , ex. Message + ":CreateCommandHelper" );
            }
        }

        /// <summary>
        /// 创建完整的指令，带CRC校验码，可直接发送给串口
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">报文长度</param>
        /// <param name="data">传入的浮点型数据</param>
        /// <returns>完整的指令，带CRC校验码，可直接发送给串口</returns>
        private OperateResult<byte[]> CreateCommandHelper ( byte commandCode , ushort commandLength , float[] data )
        {
            try
            {
                //将浮点型数据转换成字节数组
                byte[] dataBytes = ByteTransform. TransByte ( data );

                //创建完整的指令报文
                OperateResult<byte[]> commandBytes = CreateCommandHelper ( commandCode , commandLength , dataBytes );
                return commandBytes;
            }
            catch ( Exception ex )
            {
                return new OperateResult<byte[]> ( 811205 , ex. Message + "8112:CreateCommandHelper" );
            }
        }

        /// <summary>
        /// 创建完整的指令，带CRC校验码，可直接发送给串口。
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">报文长度</param>
        /// <param name="data">传入的浮点型数据</param>
        /// <param name="Flag"></param>
        /// <returns>完整的指令，带CRC校验码，可直接发送给串口</returns>
        private OperateResult<byte[]> CreateCommandHelper ( byte commandCode , ushort commandLength , float[] data , byte Flag )
        {
            try
            {
                //将浮点型数据转换成字节数组
                byte[] datas = ByteTransform. TransByte ( data );

                //将Flag数据和data合并在一起
                byte[] dataBytes = new byte[datas. Length + 1];
                Array. Copy ( datas , dataBytes , datas. Length );
                dataBytes[dataBytes. Length - 1] = Flag;

                //创建完整的指令报文
                OperateResult<byte[]> commandBytes = CreateCommandHelper ( commandCode , commandLength , dataBytes );
                return commandBytes;
            }
            catch ( Exception ex )
            {
                return new OperateResult<byte[]> ( 811205 , ex. Message + "8112:CreateCommandHelper" );
            }
        }

        /// <summary>
        /// 谐波参数转化
        /// </summary>
        /// <param name="harmonic"></param>
        /// <returns></returns>
        private byte[] HarmonicToBytes ( Harmonics harmonic )
        {
            byte[] bytes = new byte[9];
            bytes[0] = harmonic. HarmonicTimes;

            //将谐波幅度转换为字节数组并复制到目标
            ByteTransform. TransByte ( harmonic. Amplitude ). CopyTo ( bytes , 1 );

            //将谐波相位转换为字节数组并复制到目标
            ByteTransform. TransByte ( harmonic. Angle ). CopyTo ( bytes , 5 );

            return bytes;
        }


        #endregion Private CommandBuilder Helper 【报文创建助手】

        #region --------------------------------- private CommandBuilder【报文创建】---------------

        #region 系统命令【报文创建】
        /// <summary>
        /// 创建一个【联机指令】原始报文
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateHandShake ( )
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper ( DK81CommunicationInfo. HandShake , DK81CommunicationInfo. HandShakeCommandLength );

            return bytesHeader;
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateSetSystemMode ( SystemMode mode )
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper ( DK81CommunicationInfo. SetSystemMode , DK81CommunicationInfo. SetSystemModeCommandLength , mode );
            return bytesHeader;
        }

        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateSetDisplayPage ( DisplayPage page )    //TODO 将DisplayPage写成属性
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper ( DK81CommunicationInfo. SetDisplayPage , DK81CommunicationInfo. SetDisplayPageCommandLength , page );
            return bytesHeader;
        }
        #endregion 系统命令【报文创建】           

        #region 交流表源命令【报文创建】
        /// <summary>
        /// 创建读取交流标准源和标准表档位信息报文
        /// </summary>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadACSourceRanges ( )
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper ( DK81CommunicationInfo. ReadACSourceRanges , DK81CommunicationInfo. ReadACSourceRangesLength );

            return bytesHeader;
        }

        /// <summary>
        /// 创建一个【交流源关闭】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateStopACSource ( )
        {
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. StopACSource , DK81CommunicationInfo. StopACSourceLength );

            return bytes;
        }

        /// <summary>
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateStartACSource ( )
        {
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. StartACSource , DK81CommunicationInfo. StartACSourceLength );

            return bytes;
        }

        /// <summary>
        /// 创建【交流源档位设置】报文，返回带信息的报文
        /// </summary>
        /// <param name="urange">交流电压档位</param>
        /// <param name="irange">交流电流档位</param>
        /// <param name="ipranges">保护电流档位</param>
        /// <returns></returns>
        private OperateResult<byte[]> CreatSetACSourceRange ( int urange , int irange , int ipranges )
        {
            try
            {
                byte[] ranges = new byte[9];
                for ( int i = 0 ; i < 3 ; i++ )
                {
                    ranges[i] = ( byte ) urange;
                    ranges[i + 3] = ( byte ) irange;
                    ranges[i + 6] = ( byte ) ipranges;
                }

                OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. SetACSourceRanges , DK81CommunicationInfo. SetRangesLength , ranges );
                return bytes;
            }
            catch ( Exception ex )
            {

                return new OperateResult<byte[]> ( 8112 , "请输入正确的档位索引值，0为最大档位" + ex. Message );
            }

        }

        /// <summary>
        /// 创建【设置交流源幅度】的完整报文
        /// </summary>
        /// <param name="data">将要设置的浮点型幅值</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteACSourceAmplitude ( float[] data )
        {
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WriteACSourceAmplitude , DK81CommunicationInfo. WriteACSourceAmplitudeLength , data );
            return bytes;
        }

        /// <summary>
        /// 创建【设置源相位】的完整报文，返回OK
        /// </summary>
        /// <param name="data">浮点数组：PhaseUa，PhaseUb，PhaseUc，PhaseIa，PhaseIb，PhaseIc</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWritePhase ( float[] data )
        {
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WritePhase , DK81CommunicationInfo. WritePhaseLength , data );
            return bytes;
        }

        /// <summary>
        /// 创建【设置源频率】的完整报文，返回OK
        /// </summary>
        /// <param name="data">浮点型数组</param>
        /// <param name="Flag">标志</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteFrequency ( float[] data , byte Flag )
        {
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WriteFrequency , DK81CommunicationInfo. WriteFrequencyLength , data , Flag );
            return bytes;
        }

        /// <summary>
        /// 创建【设置接线模式】完整报文
        /// </summary>
        /// <param name="wireMode">接线模式</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateSetWireMode ( WireMode wireMode )
        {
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. SetWireMode , DK81CommunicationInfo. SetWireModeLength , wireMode );
            return bytes;
        }

        /// <summary>
        ///  创建【闭环控制】完整报文
        /// </summary>
        /// <param name="closeLoopMode">闭环模式枚举</param>
        /// <param name="harmonicMode">谐波模式枚举</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateSetClosedLoop ( CloseLoopMode closeLoopMode , HarmonicMode harmonicMode )
        {
            byte[] data = new byte[2] { ( byte ) closeLoopMode , ( byte ) harmonicMode };
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. SetClosedLoop , DK81CommunicationInfo. SetClosedLoopLength , data );
            return bytes;
        }

        /// <summary>
        ///  创建【设置谐波】完整报文
        /// </summary>
        /// <param name="harmonicChannels">谐波通道选择</param>
        /// <param name="harmonics">谐波参数</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteHarmonics ( ChannelsHarmonic harmonicChannels , Harmonics[] harmonics )
        {
            byte count = ( byte ) harmonics. Length;    //要设置的谐波个数
            ushort dataLength = ( ushort ) ( 2 + count * 9 );     //数据区长度
            byte[] data = new byte[dataLength];              //数据区报文

            byte[] harmonicsData = new byte[count * 9]; //谐波参数报文

            //所有谐波数据转换成字节数组
            for ( int i = 0 ; i < count ; i++ )
            {
                HarmonicToBytes ( harmonics[i] ). CopyTo ( harmonicsData , i * 9 );
            }

            //拼装数据区报文
            data[0] = ( byte ) harmonicChannels;
            data[1] = count;
            harmonicsData. CopyTo ( data , 2 );

            //创建完整报文            
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WriteHarmonics , ( ushort ) ( dataLength + 7 ) , data );
            return bytes;
        }

        /// <summary>
        /// 创建【清空谐波】报文
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteHarmonicsClear ( ChannelsHarmonic channelsHarmonic )
        {
            byte[] data = new byte[2] { ( byte ) channelsHarmonic , 0 };    //COUNT=0表示清空指定通道的谐波
            return CreateCommandHelper ( DK81CommunicationInfo. WriteHarmonics , DK81CommunicationInfo. WriteHarmonicsClearLength , data );
        }

        /// <summary>
        /// 创建【设置有功功率】报文
        /// </summary>
        /// <param name="channel">0-Pa，1-Pb，2-Pc，3-ΣP</param>
        /// <param name="p">有功功率设定值</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteWattPower ( ChannelWattPower channel , float p )
        {
            byte[] data = new byte[5];
            data[0] = ( byte ) channel;
            ByteTransform. TransByte ( p ). CopyTo ( data , 1 );

            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WriteWattPower , DK81CommunicationInfo. WriteWattPowerLength , data );
            return bytes;
        }

        /// <summary>
        /// 创建【设置无功功率】报文
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="p"></param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteWattLessPower ( ChannelWattLessPower channel , float p )
        {
            byte[] data = new byte[5];
            data[0] = ( byte ) channel;
            ByteTransform. TransByte ( p ). CopyTo ( data , 1 );

            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WriteWattlessPower , DK81CommunicationInfo. WriteWattlessPowerLength , data );
            return bytes;
        }

        /// <summary>
        /// 创建【读取当前交流源输出值】报文
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateReadACSourceData ( )
        {

            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. ReadACSourceData , DK81CommunicationInfo. ReadACSourceDataLength );
            return bytes;
        }

        /// <summary>
        /// 创建【读取当前交流源输出状态】报文
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateReadACStatus ( )
        {

            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. ReadACStatus , DK81CommunicationInfo. ReadACStatusLength );
            return bytes;
        }
        #endregion 交流表源命令【报文创建】 

        #region 电能【报文创建】 
        /// <summary>
        /// 创建【设置电能校验参数并启动电能校验】完整报文
        /// </summary>
        /// <param name="electricityType"></param>
        /// <param name="meterPConst"></param>
        /// <param name="meterQConst"></param>
        /// <param name="sourcePConst"></param>
        /// <param name="sourceQConst"></param>
        /// <param name="meterDIV"></param>
        /// <param name="meterRounds"></param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteElectricity ( ElectricityType electricityType , float meterPConst , float meterQConst , float sourcePConst , float sourceQConst , uint meterDIV , uint meterRounds )
        {
            //数据区字节数组
            byte[] data = new byte[25];

            //拼装数据区
            data[0] = ( byte ) electricityType;
            ByteTransform. TransByte ( meterPConst ). CopyTo ( data , 1 );
            ByteTransform. TransByte ( meterQConst ). CopyTo ( data , 5 );
            ByteTransform. TransByte ( sourcePConst ). CopyTo ( data , 9 );
            ByteTransform. TransByte ( sourceQConst ). CopyTo ( data , 13 );
            BitConverter. GetBytes ( meterDIV ). CopyTo ( data , 17 );
            BitConverter. GetBytes ( meterRounds ). CopyTo ( data , 21 );

            //返回创建好的完整报文
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. WriteElectricity , DK81CommunicationInfo. WriteElectricityLength , data );
            return bytes;
        }

        /// <summary>
        /// 创建【读取电能误差】命令报文
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateReadElectricityDeviation ( )
        {

            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. ReadElectricityDeviation , DK81CommunicationInfo. ReadElectricityDeviationLength );
            return bytes;
        }
        #endregion 电能报文创建

        #region 直流表【报文创建】
        /// <summary>
        /// 创建【设置直流表量程】命令报文
        /// </summary>
        /// <param name="rangeIndex">直流表档位索引字</param>
        /// <param name="type">直流表测量类型</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateSetDCMeterRange ( byte rangeIndex , DCMerterMeasureType type )
        {
            byte[] data = new byte[2];
            data[0] = ( byte ) rangeIndex;
            data[1] = ( byte ) type;
            OperateResult<byte[]> bytes = CreateCommandHelper ( DK81CommunicationInfo. SetDCMeterRange , DK81CommunicationInfo. SetDCMeterRangeLength , data );
            return bytes;
        }

        /// <summary>
        /// 创建【读取直流表数据】报文
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateReadDCMeterData ( )
        {
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. ReadDCMeterData , DK81CommunicationInfo. ReadDCMeterDataLength );
            return result;
        }

        /// <summary>
        /// 创建读取直流表档位信息的报文
        /// </summary>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadDCMeterRanges ( )
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper ( DK81CommunicationInfo. ReadDCMeterRanges , DK81CommunicationInfo. ReadDCMeterRangesLength );

            return bytesHeader;
        }
        #endregion 直流表报文创建

        #region 直流源【报文创建】

        /// <summary>
        /// 创建读取直流源档位信息的报文
        /// </summary>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadDCSourceRanges ( )
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper ( DK81CommunicationInfo. ReadDCSourceRanges , DK81CommunicationInfo. ReadDCSourceRangesLength );

            return bytesHeader;
        }
        /// <summary>
        /// 创建【直流源打开命令】报文
        /// </summary>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateStartDCSource ( DCSourceType dCSourceType )
        {
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. StartDCSource , DK81CommunicationInfo. StartDCSourceLength , dCSourceType );
            return result;
        }

        /// <summary>
        /// 创建【直流源关闭命令】报文
        /// </summary>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateStopDCSource ( DCSourceType dCSourceType )
        {
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. StopDCSource , DK81CommunicationInfo. StopDCSourceLength , dCSourceType );
            return result;
        }

        /// <summary>
        /// 创建【设置直流源档位参数】报文
        /// </summary>
        /// <param name="rangeIndex">当前档位索引值</param>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateSetDCSourceRange ( byte rangeIndex , DCSourceType dCSourceType )
        {
            byte[] data = new byte[2];
            data[0] = rangeIndex;
            data[1] = ( byte ) ( dCSourceType );

            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. SetDCSourceRange , DK81CommunicationInfo. SetDCSourceRangeLength , data );
            return result;
        }

        /// <summary>
        /// 创建【设置直流源幅值】报文
        /// </summary>
        /// <param name="rangeIndex"></param>
        /// <param name="SData"></param>
        /// <param name="dCSourceType"></param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteDCSourceAmplitude ( byte rangeIndex , float SData , DCSourceType dCSourceType )
        {
            byte[] data = new byte[6];
            data[0] = rangeIndex;
            ByteTransform. TransByte ( SData ). CopyTo ( data , 1 );
            data[5] = ( byte ) ( dCSourceType );

            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. WriteDCSourceAmplitude , DK81CommunicationInfo. WriteDCSourceAmplitudeLength , data );
            return result;
        }

        /// <summary>
        /// 创建【读取直流源参数】报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateReadDCSourceData ( DCSourceType dCSourceType )
        {
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. ReadDCSourceData , DK81CommunicationInfo. ReadDCSourceDataLength , dCSourceType );
            return result;
        }

        #endregion

        #region 校准指令【报文创建】
        /// <summary>
        /// 创建【清空校准数据】报文
        /// </summary>
        /// <param name="calibrateType">校准时的操作类型</param>
        /// <param name="uRangeIndex">电压档位</param>
        /// <param name="iRangeIndex">电流档位</param>
        /// <returns>带成功标志的操作结果：创建的完整报文</returns>
        private OperateResult<byte[]> CreateCalibrate_ClearData ( CalibrateType calibrateType , byte uRangeIndex , byte iRangeIndex )
        {
            byte[] data = new byte[3];
            data[0] = ( byte ) calibrateType;
            data[1] = uRangeIndex;
            data[2] = iRangeIndex;
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_ClearData , DK81CommunicationInfo. Calibrate_ClearDataLength , data );
            return result;
        }

        /// <summary>
        /// 创建【切换交流源（表）校准档位】报文
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值</param>
        /// <param name="iRangeIndex">电流档位索引值</param>
        /// <returns>带成功标志的操作结果：创建的完整报文</returns>
        private OperateResult<byte[]> CreateCalibrate_SwitchACRange ( byte uRangeIndex , byte iRangeIndex )
        {
            byte[] data = new byte[2];
            data[0] = uRangeIndex;
            data[1] = iRangeIndex;
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_SwitchACRange , DK81CommunicationInfo. Calibrate_SwitchACRangeLength , data );
            return result;
        }

        /// <summary>
        /// 创建【设置交流源（表）校准点数据】报文
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值</param>
        /// <param name="iRangeIndex">电流档位索引值</param>
        /// <returns>带成功标志的操作结果：创建的完整报文</returns>
        private OperateResult<byte[]> CreateCalibrate_SwitchACPoint ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel , float sUA , float sUB , float sUC , float sIA , float sIB , float sIC )
        {
            byte[] data = new byte[27];
            data[0] = uRangeIndex;
            data[1] = iRangeIndex;
            data[2] = ( byte ) calibrateLevel;
            ByteTransform. TransByte ( sUA ). CopyTo ( data , 3 );
            ByteTransform. TransByte ( sUB ). CopyTo ( data , 7 );
            ByteTransform. TransByte ( sUC ). CopyTo ( data , 11 );
            ByteTransform. TransByte ( sIA ). CopyTo ( data , 15 );
            ByteTransform. TransByte ( sIB ). CopyTo ( data , 19 );
            ByteTransform. TransByte ( sIC ). CopyTo ( data , 23 );

            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_SwitchACPoint , DK81CommunicationInfo. Calibrate_SwitchACPointLength , data );
            return result;
        }

        /// <summary>
        /// 创建【执行交流源校准】报文
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值，校验用</param>
        /// <param name="iRangeIndex">电流档位索引值，校验用</param>
        /// <param name="calibrateLevel"></param>
        /// <param name="mUA">当前所接的标准表的读数</param>
        /// <param name="mUB">当前所接的标准表的读数</param>
        /// <param name="mUC">当前所接的标准表的读数</param>
        /// <param name="mIA">当前所接的标准表的读数</param>
        /// <param name="mIB">当前所接的标准表的读数</param>
        /// <param name="mIC">当前所接的标准表的读数</param>
        /// <returns>带成功标志的操作结果：创建的完整报文</returns>
        private OperateResult<byte[]> CreateCalibrate_DoAC ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel , float mUA , float mUB , float mUC , float mIA , float mIB , float mIC )
        {
            byte[] data = new byte[27];
            data[0] = uRangeIndex;
            data[1] = iRangeIndex;
            data[2] = ( byte ) calibrateLevel;
            ByteTransform. TransByte ( mUA ). CopyTo ( data , 3 );
            ByteTransform. TransByte ( mUB ). CopyTo ( data , 7 );
            ByteTransform. TransByte ( mUC ). CopyTo ( data , 11 );
            ByteTransform. TransByte ( mIA ). CopyTo ( data , 15 );
            ByteTransform. TransByte ( mIB ). CopyTo ( data , 19 );
            ByteTransform. TransByte ( mIC ). CopyTo ( data , 23 );

            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_DoAC , DK81CommunicationInfo. Calibrate_DoACLength , data );
            return result;
        }

        /// <summary>
        /// 创建【确认交流源校准，保存校准参数】报文
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCalibrate_Save ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel )
        {
            byte[] data = new byte[3];
            data[0] = uRangeIndex;
            data[1] = iRangeIndex;
            data[2] = ( byte ) calibrateLevel;

            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_Save , DK81CommunicationInfo. Calibrate_SaveLength , data );
            return result;
        }

        /// <summary>
        /// 创建【交流标准表和钳形表校准】报文
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCalibrate_DoACMeter ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel , float UA , float UB , float UC , float IA , float IB , float IC )
        {
            byte[] data = new byte[27];
            data[0] = uRangeIndex;
            data[1] = iRangeIndex;
            data[2] = ( byte ) calibrateLevel;
            ByteTransform. TransByte ( UA ). CopyTo ( data , 3 );
            ByteTransform. TransByte ( UB ). CopyTo ( data , 7 );
            ByteTransform. TransByte ( UC ). CopyTo ( data , 11 );
            ByteTransform. TransByte ( IA ). CopyTo ( data , 15 );
            ByteTransform. TransByte ( IB ). CopyTo ( data , 19 );
            ByteTransform. TransByte ( IC ). CopyTo ( data , 23 );

            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_DoACMeter , DK81CommunicationInfo. Calibrate_DoACMeterlength , data );
            return result;
        }

        /// <summary>
        /// 创建【设置直流源校准点】报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCalibrate_SwitchDCPoint ( Calibrate_DCSourceType dCSourceType , byte rangeIndex , CalibrateLevel calibrateLevel , float sDCAmplitude )
        {
            if ( calibrateLevel == CalibrateLevel. 相位校准 )
            {
                return new OperateResult<byte[]> ( 8112815 , "请选择正确的校准点" );
            }
            byte[] data = new byte[7];
            data[0] = ( byte ) dCSourceType;
            data[1] = rangeIndex;
            data[2] = ( byte ) calibrateLevel;
            ByteTransform. TransByte ( sDCAmplitude ). CopyTo ( data , 3 );
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_SwitchDCPoint , DK81CommunicationInfo. Calibrate_SwitchDCPointLength , data );
            return result;
        }

        /// <summary>
        /// 创建【执行直流源校准】报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="mDCAmplitude"></param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCalibrate_DoDC ( Calibrate_DCSourceType dCSourceType , byte rangeIndex , CalibrateLevel calibrateLevel , float mDCAmplitude )
        {
            if ( calibrateLevel == CalibrateLevel. 相位校准 )
            {
                return new OperateResult<byte[]> ( 8112815 , "请选择正确的校准点" );
            }
            byte[] data = new byte[7];
            data[0] = ( byte ) dCSourceType;
            data[1] = rangeIndex;
            data[2] = ( byte ) calibrateLevel;
            ByteTransform. TransByte ( mDCAmplitude ). CopyTo ( data , 3 );
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_DoDC , DK81CommunicationInfo. Calibrate_DoDClength , data );
            return result;
        }

        /// <summary>
        /// 创建【直流表校准】报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCalibrate_DoDCMeter ( Calibrate_DCMeterType dCSourceType , byte rangeIndex , CalibrateLevel calibrateLevel , float sDCAmplitude )
        {
            if ( calibrateLevel == CalibrateLevel. 相位校准 )
            {
                return new OperateResult<byte[]> ( 8112815 , "请选择正确的校准点" );
            }
            byte[] data = new byte[7];
            data[0] = ( byte ) dCSourceType;
            data[1] = rangeIndex;
            data[2] = ( byte ) calibrateLevel;
            ByteTransform. TransByte ( sDCAmplitude ). CopyTo ( data , 3 );
            OperateResult<byte[]> result = CreateCommandHelper ( DK81CommunicationInfo. Calibrate_DoDCMeter , DK81CommunicationInfo. Calibrate_DoDCMeterLength , data );
            return result;
        }
        #endregion 校准指令【报文创建】

        #endregion private CommandBuilder报文创建

        #region --------------------------------- Core Interative 核心交互-------------------------
        protected virtual OperateResult<byte[]> CheckResponse ( byte[] send )
        {
            // 发送报文并获取回复报文
            OperateResult<byte[]> response = ReadBase ( send );

            //获取回复不成功
            if ( !response. IsSuccess )
            {
                return response;
            }

            // 长度校验
            if ( response. Content. Length < 7 )
            {
                return new OperateResult<byte[]> ( 811300 , StringResources. Language. ReceiveDataLengthTooShort + "811300" );
            }

            // 检查crc
            if ( !DK81CommunicationInfo. CheckCRC ( response. Content ) )
            {
                return new OperateResult<byte[]> ( StringResources. Language. CRCCheckFailed + SoftBasic. ByteToHexString ( response. Content , ' ' ) );
            }

            //回复OK
            if ( response. Content[5] == DK81CommunicationInfo. OK && response. Content[6] == send[5] )
            {
                return response;
            }

            // 检查是否报故障：是     //TODO 随时主动报故障的问题
            if ( response. Content[5] == DK81CommunicationInfo. ErrorCode )
            {
                return new OperateResult<byte[]> ( response. Content[6] , ( ( ErrorCode ) response. Content[6] ). ToString ( ) ); //TODO 测试第二种故障码解析:/*DK81CommunicationInfo.GetErrorMessageByErrorCode(response.Content[6])*/
            }

            //检查命令码：命令码不一致且不是OK命令
            if ( send[5] != response. Content[5] )
            {
                return new OperateResult<byte[]> ( response. Content[5] , $"Receive CommandCode Check Failed:SendCode is {send[5]},ReceivedCode is {response. Content[5]}" );
            }

            return response;
        }
        #endregion

        #region --------------------------------- internal Commands【操作命令】-------------------------

        #region 系统命令【操作命令】

        /// <summary>
        /// 执行【联机命令】并返回回复报文
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> HandshakeCommand ( )
        {
            OperateResult<byte[]> createResult = CreateHandShake ( );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【系统模式设置】并返回回复报文
        /// </summary>
        /// <param name="mode">要设置的系统模式</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetSystemModeCommand ( SystemMode mode )
        {
            OperateResult<byte[]> createResult = CreateSetSystemMode ( mode );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【显示页面设置】并返回回复报文
        /// </summary>
        /// <param name="mode">要显示的系统页面</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetDisplayPageCommand ( DisplayPage page )
        {
            OperateResult<byte[]> createResult = CreateSetDisplayPage ( page );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        #endregion 系统命令【操作命令】

        #region 设备信息【操作命令】
        /// <summary>
        /// 读取交流源档位
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadACSourceRangesCommand ( )
        {
            OperateResult<byte[]> createResult = CreateReadACSourceRanges ( );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }



        #endregion 设备信息【操作命令】        

        #region 交流源（表）【操作命令】
        /// <summary>
        /// 交流源关闭命令,返回OK
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> StopACSourceCommand ( )
        {
            OperateResult<byte[]> createResult = CreateStopACSource ( );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 交流源关闭命令,返回OK
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> StartACSourceCommand ( )
        {
            OperateResult<byte[]> createResult = CreateStartACSource ( );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【设置交流源档位】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetACSourceRangeCommand ( int urange , int irange , int ipranges )
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreatSetACSourceRange ( urange , irange , ipranges );

            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【设置交流源幅值】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="data">操作结果信息</param>
        /// <returns></returns>
        internal OperateResult<byte[]> WriteACSourceAmplitudeCommand ( float[] data )
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreateWriteACSourceAmplitude ( data );

            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【设置源相位】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="data">6个浮点数据：PhaseUA（基准相位必须是0）,PhaseUB,PhaseUC,PhaseIA,PhaseIB,PhaseIC</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WritePhaseCommand ( float[] data )
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreateWritePhase ( data );

            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【设置源频率】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="data">浮点数组：FrequencyA，FrequencyB(必须等于A相)，FrequencyC</param>
        /// <param name="Flag">标志</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WriteFrequencyCommand ( float[] data , byte Flag )
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreateWriteFrequency ( data , Flag );

            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【设置接线模式】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="wireMode">枚举接线方式</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> SetWireModeCommmand ( WireMode wireMode )
        {
            OperateResult<byte[]> createResult = CreateSetWireMode ( wireMode );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        ///  执行【闭环控制】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="closeLoopMode">闭环模式枚举</param>
        /// <param name="harmonicMode">谐波模式枚举</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> SetClosedLoopCommmand ( CloseLoopMode closeLoopMode , HarmonicMode harmonicMode )
        {
            OperateResult<byte[]> createResult = CreateSetClosedLoop ( closeLoopMode , harmonicMode );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【谐波参数设置】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="harmonicChannels">谐波通道</param>
        /// <param name="harmonics">谐波参数</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WriteHarmonicsCommmand ( ChannelsHarmonic harmonicChannels , Harmonics[] harmonics )
        {
            OperateResult<byte[]> createResult = CreateWriteHarmonics ( harmonicChannels , harmonics );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【清空谐波】命令
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WriteHarmonicsClearCommmand ( ChannelsHarmonic channelsHarmonic )
        {
            OperateResult<byte[]> createResult = CreateWriteHarmonicsClear ( channelsHarmonic );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【设置有功功率】命令
        /// </summary>
        /// <param name="channel">0-Pa，1-Pb，2-Pc，3-ΣP</param>
        /// <param name="p">有功功率设定值</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WriteWattPowerCommmand ( ChannelWattPower channel , float p )
        {
            OperateResult<byte[]> createResult = CreateWriteWattPower ( channel , p );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【设置无功功率】命令
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        internal OperateResult<byte[]> WriteWattPowerLessCommmand ( ChannelWattLessPower channel , float p )
        {
            OperateResult<byte[]> createResult = CreateWriteWattLessPower ( channel , p );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【读取交流源当前输出值】命令，并获取下位机回复报文
        /// </summary>
        /// <returns></returns>
        internal OperateResult<byte[]> ReadACSourceDataCommmand ( )
        {
            OperateResult<byte[]> createResult = CreateReadACSourceData ( );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        /// <summary>
        /// 执行【读取交流源当前输出状态】命令，并获取下位机回复报文
        /// </summary>
        /// <returns></returns>
        internal OperateResult<byte[]> ReadACStatusCommmand ( )
        {
            OperateResult<byte[]> createResult = CreateReadACStatus ( );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }


        #endregion 交流源（表）操作命令

        #region 电能 【操作命令】
        internal OperateResult<byte[]> WriteElectricityCommmand ( ElectricityType electricityType , float meterPConst , float meterQConst , float sourcePConst , float sourceQConst , uint meterDIV , uint meterRounds )
        {
            OperateResult<byte[]> createResult = CreateWriteElectricity ( electricityType , meterPConst , meterQConst , sourcePConst , sourceQConst , meterDIV , meterRounds );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }

        internal OperateResult<byte[]> ReadElectricityDeviationCommmand ( )
        {
            OperateResult<byte[]> createResult = CreateReadElectricityDeviation ( );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse ( createResult. Content );
            return response;
        }
        #endregion 电能操作命令

        #region 直流表 【操作命令】

        /// <summary>
        /// 执行【设置直流表量程】命令并返回下位机回复报文
        /// </summary>
        /// <param name="rangeIndex">当前直流表档位索引字</param>
        /// <param name="type">直流表测量类型</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> SetDCMeterRangeCommand ( byte rangeIndex , DCMerterMeasureType type )
        {
            OperateResult<byte[]> result = CreateSetDCMeterRange ( rangeIndex , type );
            if ( !result. IsSuccess )
            {
                return result;
            }
            //发送报文，接收并校验下位机回复报文
            OperateResult<byte[]> response = CheckResponse ( result. Content );
            return response;
        }

        /// <summary>
        ///  执行【读取直流表数据】命令并返回下位机回复报文
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> ReadDCMeterDataCommand ( )
        {
            OperateResult<byte[]> result = CreateReadDCMeterData ( );
            if ( !result. IsSuccess )
            {
                return result;
            }
            //发送报文，接收并校验下位机回复报文
            OperateResult<byte[]> response = CheckResponse ( result. Content );
            return response;
        }

        /// <summary>
        /// 读取直流表档位
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadDCMeterRangesCommand ( )
        {
            OperateResult<byte[]> createResult = CreateReadDCMeterRanges ( );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }
        #endregion 直流表操作命令

        #region 【直流源】操作命令

        /// <summary>
        /// 读取直流源档位
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadDCSourceRangesCommand ( )
        {
            OperateResult<byte[]> createResult = CreateReadDCSourceRanges ( );
            //创建指令失败
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【打开直流源】命令并返回下位机回复报文
        /// </summary>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> StartDCSourceCommand ( DCSourceType dCSourceType )
        {
            OperateResult<byte[]> createResult = CreateStartDCSource ( dCSourceType );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【关闭直流源】命令并返回下位机回复报文
        /// </summary>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> StopDCSourceCommand ( DCSourceType dCSourceType )
        {
            OperateResult<byte[]> createResult = CreateStopDCSource ( dCSourceType );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【设置直流源档位】命令并返回下位机回复报文
        /// </summary>
        /// <param name="rangeIndex">当前档位索引值</param>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetDCSourceRangeCommand ( byte rangeIndex , DCSourceType dCSourceType )
        {
            OperateResult<byte[]> createResult = CreateSetDCSourceRange ( rangeIndex , dCSourceType );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【设置直流源幅值】命令并返回下位机回复报文
        /// </summary>
        /// <param name="rangeIndex"></param>
        /// <param name="SData"></param>
        /// <param name="dCSourceType"></param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> WriteDCSourceAmplitudeCommand ( byte rangeIndex , float SData , DCSourceType dCSourceType )
        {
            OperateResult<byte[]> createResult = CreateWriteDCSourceAmplitude ( rangeIndex , SData , dCSourceType );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【读取直流源参数】命令并返回下位机回复报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadDCSourceDataCommand ( DCSourceType dCSourceType )
        {
            OperateResult<byte[]> createResult = CreateReadDCSourceData ( dCSourceType );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }
        #endregion 【直流源】操作命令

        #region 【校准】 操作命令
        /// <summary>
        ///  执行【清空校准数据】命令并返回下位机回复报文
        /// </summary>
        /// <param name="calibrateType">校准时的操作类型</param>
        /// <param name="uRangeIndex">电压档位索引值</param>
        /// <param name="iRangeIndex">电流档位索引值</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> Calibrate_ClearDataCommand ( CalibrateType calibrateType , byte uRangeIndex , byte iRangeIndex )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_ClearData ( calibrateType , uRangeIndex , iRangeIndex );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【切换交流源（表）校准档位】命令并返回下位机回复报文
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值</param>
        /// <param name="iRangeIndex">电流档位索引值</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> Calibrate_SwitchACRangeCommand ( byte uRangeIndex , byte iRangeIndex )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_SwitchACRange ( uRangeIndex , iRangeIndex );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【切换交流源（表）校准点】命令并返回下位机回复报文
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值</param>
        /// <param name="iRangeIndex">电流档位索引值</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> Calibrate_SwitchACPointCommand ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel , float sUA , float sUB , float sUC , float sIA , float sIB , float sIC )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_SwitchACPoint ( uRangeIndex , iRangeIndex , calibrateLevel , sUA , sUB , sUC , sIA , sIB , sIC );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【执行交流源校准】命令并返回下位机回复报文
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值，校验用</param>
        /// <param name="iRangeIndex">电流档位索引值，校验用</param>
        /// <param name="calibrateLevel"></param>
        /// <param name="mUA">当前所接的标准表的读数</param>
        /// <param name="mUB">当前所接的标准表的读数</param>
        /// <param name="mUC">当前所接的标准表的读数</param>
        /// <param name="mIA">当前所接的标准表的读数</param>
        /// <param name="mIB">当前所接的标准表的读数</param>
        /// <param name="mIC">当前所接的标准表的读数</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> Calibrate_DoACCommand ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel , float mUA , float mUB , float mUC , float mIA , float mIB , float mIC )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_DoAC ( uRangeIndex , iRangeIndex , calibrateLevel , mUA , mUB , mUC , mIA , mIB , mIC );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【确认交流源校准，保存校准参数】命令并返回下位机回复报文
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <returns></returns>
        internal OperateResult<byte[]> Calibrate_SaveCommand ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_Save ( uRangeIndex , iRangeIndex , calibrateLevel );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【交流标准表和钳形表校准】命令并返回下位机回复报文
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <returns></returns>
        internal OperateResult<byte[]> Calibrate_DoACMeterCommand ( byte uRangeIndex , byte iRangeIndex , CalibrateLevel calibrateLevel , float UA , float UB , float UC , float IA , float IB , float IC )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_DoACMeter ( uRangeIndex , iRangeIndex , calibrateLevel , UA , UB , UC , IA , IB , IC );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        internal OperateResult<byte[]> Calibrate_SwitchDCPointCommand ( Calibrate_DCSourceType dCSourceType , byte rangeIndex , CalibrateLevel calibrateLevel , float sDCAmplitude )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_SwitchDCPoint ( dCSourceType , rangeIndex , calibrateLevel , sDCAmplitude );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【执行直流源校准】命令并返回下位机回复报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        internal OperateResult<byte[]> Calibrate_DoDCCommand ( Calibrate_DCSourceType dCSourceType , byte rangeIndex , CalibrateLevel calibrateLevel , float mDCAmplitude )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_DoDC ( dCSourceType , rangeIndex , calibrateLevel , mDCAmplitude );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }

        /// <summary>
        /// 执行【直流表校准】命令并返回下位机回复报文
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        internal OperateResult<byte[]> Calibrate_DoDCMeterCommand ( Calibrate_DCMeterType dCSourceType , byte rangeIndex , CalibrateLevel calibrateLevel , float sDCAmplitude )
        {
            OperateResult<byte[]> createResult = CreateCalibrate_DoDCMeter ( dCSourceType , rangeIndex , calibrateLevel , sDCAmplitude );
            if ( !createResult. IsSuccess )
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse ( createResult. Content );
            return responseBytes;
        }
        #endregion 【校准】 操作命令
        #endregion internal Commands操作命令

    }
}

