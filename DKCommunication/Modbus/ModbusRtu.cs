﻿using DKCommunication.BasicFramework;
using DKCommunication.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DKCommunication.Core;
using DKCommunication.Core.Address;

namespace DKCommunication.ModBus
{
    
    public class ModbusRtu : SerialDeviceBase<ReverseWordTransform>
    {
        #region Constructor

        /// <summary>
        /// 实例化一个Modbus-Rtu协议的客户端对象
        /// </summary>
        public ModbusRtu( )
        {
            ByteTransform = new ReverseWordTransform( );
        }


        /// <summary>
        /// 指定服务器地址，端口号，客户端自己的站号来初始化
        /// </summary>
        /// <param name="station">客户端自身的站号</param>
        public ModbusRtu( byte station = 0x01 )
        {
            ByteTransform = new ReverseWordTransform( );
            this.station = station;
        }

        #endregion

        #region Private Member

        private byte station = ModbusInfo.ReadCoil;                  // 本客户端的站号
        private bool isAddressStartWithZero = true;                  // 线圈值的地址值是否从零开始

        #endregion

        #region Public Member
        
        /// <summary>
        /// 获取或设置起始的地址是否从0开始，默认为True
        /// </summary>
        /// <remarks>
        /// <note type="warning">因为有些设备的起始地址是从1开始的，就要设置本属性为<c>True</c></note>
        /// </remarks>
        public bool AddressStartWithZero
        {
            get { return isAddressStartWithZero; }
            set { isAddressStartWithZero = value; }
        }

        /// <summary>
        /// 获取或者重新修改服务器的默认站号信息
        /// </summary>
        /// <remarks>
        /// 当你调用 ReadCoil("100") 时，对应的站号就是本属性的值，当你调用 ReadCoil("s=2;100") 时，就忽略本属性的值，读写寄存器的时候同理
        /// </remarks>
        public byte Station
        {
            get { return station; }
            set { station = value; }
        }
        
        /// <summary>
        /// 获取或设置数据解析的格式，默认ABCD，可选BADC，CDAB，DCBA格式
        /// </summary>
        /// <remarks>
        /// 对于Int32,UInt32,float,double,Int64,UInt64类型来说，存在多地址的电脑情况，需要和服务器进行匹配
        /// </remarks>
        public DataFormat DataFormat
        {
            get { return ByteTransform.DataFormat; }
            set { ByteTransform.DataFormat = value; }
        }

        /// <summary>
        /// 字符串数据是否按照字来反转
        /// </summary>
        /// <remarks>
        /// 字符串按照2个字节的排列进行颠倒，根据实际情况进行设置
        /// </remarks>
        public bool IsStringReverse
        {
            get { return ByteTransform.IsStringReverse; }
            set { ByteTransform.IsStringReverse = value; }
        }
        
        #endregion

        #region Build Command
        
        /// <summary>
        /// 生成一个读取线圈的指令头
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">长度</param>
        /// <returns>携带有命令字节</returns>
        public OperateResult<byte[]> BuildReadCoilCommand( string address, ushort count )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终tcp指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateReadCoils( station, count ) );
            return OperateResult.CreateSuccessResult( buffer );
        }

        /// <summary>
        /// 生成一个读取离散信息的指令头
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns>携带有命令字节</returns>
        public OperateResult<byte[]> BuildReadDiscreteCommand( string address, ushort length )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终tcp指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateReadDiscrete( station, length ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        /// <summary>
        /// 生成一个读取寄存器的指令头
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns>携带有命令字节</returns>
        public OperateResult<byte[]> BuildReadRegisterCommand( string address, ushort length )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终rtu指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateReadRegister( station, length ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        /// <summary>
        /// 生成一个读取寄存器的指令头
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns>携带有命令字节</returns>
        private OperateResult<byte[]> BuildReadRegisterCommand( ModbusAddress address, ushort length )
        {
            // 生成最终rtu指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( address.CreateReadRegister( station, length ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        /// <summary>
        /// 生成一个写入单线圈的指令头
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">长度</param>
        /// <returns>包含结果对象的报文</returns>
        public OperateResult<byte[]> BuildWriteOneCoilCommand( string address, bool value )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终rtu指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateWriteOneCoil( station, value ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        /// <summary>
        /// 生成一个写入单个寄存器的报文
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="data">长度</param>
        /// <returns>包含结果对象的报文</returns>
        public OperateResult<byte[]> BuildWriteOneRegisterCommand( string address, byte[] data )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终rtu指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateWriteOneRegister( station, data ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        /// <summary>
        /// 生成批量写入单个线圈的报文信息
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="values">实际数据值</param>
        /// <returns>包含结果对象的报文</returns>
        public OperateResult<byte[]> BuildWriteCoilCommand( string address, bool[] values )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终rtu指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateWriteCoil( station, values ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        /// <summary>
        /// 生成批量写入寄存器的报文信息
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="values">实际值</param>
        /// <returns>包含结果对象的报文</returns>
        public OperateResult<byte[]> BuildWriteRegisterCommand( string address, byte[] values )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );
            
            // 生成最终rtu指令
            byte[] buffer = ModbusInfo.PackCommandToRtu( analysis.Content.CreateWriteRegister( station, values ) );
            return OperateResult.CreateSuccessResult( buffer );
        }
        
        #endregion
        
        #region Core Interative
        
        /// <summary>
        /// 检查当前的Modbus-Rtu响应是否是正确的
        /// </summary>
        /// <param name="send">发送的数据信息</param>
        /// <returns>带是否成功的结果数据</returns>
        protected virtual OperateResult<byte[]> CheckModbusTcpResponse( byte[] send )
        {
            // 核心交互
            OperateResult<byte[]> result = ReadBase( send );
            if (!result.IsSuccess) return result;

            // 长度校验
            if (result.Content.Length < 5) return new OperateResult<byte[]>( StringResources.Language.ReceiveDataLengthTooShort + "5" );

            // 检查crc
            if (!SoftCRC16.CheckCRC16( result.Content )) return new OperateResult<byte[]>( StringResources.Language.ModbusCRCCheckFailed +
                SoftBasic.ByteToHexString( result.Content, ' ' ) );

            // 发生了错误
            if ((send[1] + 0x80) == result.Content[1]) return new OperateResult<byte[]>( result.Content[2], ModbusInfo.GetDescriptionByErrorCode( result.Content[2] ) );

            if (send[1] != result.Content[1]) return new OperateResult<byte[]>( result.Content[1], $"Receive Command Check Failed: " );

            // 移除CRC校验
            byte[] buffer = new byte[result.Content.Length - 2];
            Array.Copy( result.Content, 0, buffer, 0, buffer.Length );
            return OperateResult.CreateSuccessResult( buffer );
            
        }

        #endregion

        #region Protect Override
        /// <summary>
        /// 检查当前接收的字节数据是否正确的
        /// </summary>
        /// <param name="rBytes">从设备反馈回来的数据</param>
        /// <returns>是否校验成功</returns>
        protected override bool CheckReceivedBytes( byte[] rBytes )
        {
            return SoftCRC16.CheckCRC16( rBytes );
        }
        #endregion

        #region Read Support

        /// <summary>
        /// 读取服务器的数据，需要指定不同的功能码
        /// </summary>
        /// <param name="code">指令</param>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns>带结果信息的字节返回数据</returns>
        protected OperateResult<byte[]> ReadModBusBase( byte code, string address, ushort length )
        {
            OperateResult<byte[]> command;
            switch (code)
            {
                case ModbusInfo.ReadCoil:
                    {
                        command = BuildReadCoilCommand( address, length );
                        break;
                    }
                case ModbusInfo.ReadDiscrete:
                    {
                        command = BuildReadDiscreteCommand( address, length );
                        break;
                    }
                case ModbusInfo.ReadRegister:
                    {
                        command = BuildReadRegisterCommand( address, length );
                        break;
                    }
                default: command = new OperateResult<byte[]>( ) { Message = StringResources.Language.ModbusTcpFunctionCodeNotSupport }; break;
            }
            if (!command.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( command );

            OperateResult<byte[]> resultBytes = CheckModbusTcpResponse( command.Content );
            if (resultBytes.IsSuccess)
            {
                // 二次数据处理
                if (resultBytes.Content?.Length >= 3)
                {
                    byte[] buffer = new byte[resultBytes.Content.Length - 3];
                    Array.Copy( resultBytes.Content, 3, buffer, 0, buffer.Length );
                    resultBytes.Content = buffer;
                }
            }
            return resultBytes;
        }

        /// <summary>
        /// 读取服务器的数据，需要指定不同的功能码
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">长度</param>
        /// <returns>带结果信息的字节返回数据</returns>
        protected OperateResult<byte[]> ReadModBusBase( ModbusAddress address, ushort length )
        {
            OperateResult<byte[]> command = BuildReadRegisterCommand( address, length );
            if (!command.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( command );

            OperateResult<byte[]> resultBytes = CheckModbusTcpResponse( command.Content );
            if (resultBytes.IsSuccess)
            {
                // 二次数据处理
                if (resultBytes.Content?.Length >= 3)
                {
                    byte[] buffer = new byte[resultBytes.Content.Length - 3];
                    Array.Copy( resultBytes.Content, 3, buffer, 0, buffer.Length );
                    resultBytes.Content = buffer;
                }
            }
            return resultBytes;
        }
        
        /// <summary>
        /// 读取线圈，需要指定起始地址
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <returns>带有成功标志的bool对象</returns>
        public OperateResult<bool> ReadCoil( string address )
        {
            var read = ReadCoil( address, 1 );
            if (!read.IsSuccess) return OperateResult.CreateFailedResult<bool>( read );

            return OperateResult.CreateSuccessResult( read.Content[0] );
        }
        
        /// <summary>
        /// 批量的读取线圈，需要指定起始地址，读取长度
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <param name="length">读取长度</param>
        /// <returns>带有成功标志的bool数组对象</returns>
        public OperateResult<bool[]> ReadCoil( string address, ushort length )
        {
            var read = ReadModBusBase( ModbusInfo.ReadCoil, address, length );
            if (!read.IsSuccess) return OperateResult.CreateFailedResult<bool[]>( read );

            return OperateResult.CreateSuccessResult( SoftBasic.ByteToBoolArray( read.Content, length ) );
        }
        
        /// <summary>
        /// 读取输入线圈，需要指定起始地址
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <returns>带有成功标志的bool对象</returns>
        public OperateResult<bool> ReadDiscrete( string address )
        {
            var read = ReadDiscrete( address, 1 );
            if (!read.IsSuccess) return OperateResult.CreateFailedResult<bool>( read );

            return OperateResult.CreateSuccessResult( read.Content[0] );
        }
        
        /// <summary>
        /// 批量的读取输入点，需要指定起始地址，读取长度
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <param name="length">读取长度</param>
        /// <returns>带有成功标志的bool数组对象</returns>
        public OperateResult<bool[]> ReadDiscrete( string address, ushort length )
        {
            var read = ReadModBusBase( ModbusInfo.ReadDiscrete, address, length );
            if (!read.IsSuccess) return OperateResult.CreateFailedResult<bool[]>( read );

            return OperateResult.CreateSuccessResult( SoftBasic.ByteToBoolArray( read.Content, length ) );
        }
        
        /// <summary>
        /// 从Modbus服务器批量读取寄存器的信息，需要指定起始地址，读取长度
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"，或者是带功能码格式x=3;1234</param>
        /// <param name="length">读取的数量</param>
        /// <returns>带有成功标志的字节信息</returns>
        /// <example>
        /// 此处演示批量读取的示例
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\Modbus\Modbus.cs" region="ReadExample2" title="Read示例" />
        /// </example>
        public override OperateResult<byte[]> Read( string address, ushort length )
        {
            OperateResult<ModbusAddress> analysis = ModbusInfo.AnalysisReadAddress( address, isAddressStartWithZero );
            if (!analysis.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( analysis );

            List<byte> lists = new List<byte>( );
            ushort alreadyFinished = 0;
            while (alreadyFinished < length)
            {
                ushort lengthTmp = (ushort)Math.Min( (length - alreadyFinished), 120 );
                OperateResult<byte[]> read = ReadModBusBase( analysis.Content.AddressAdd( alreadyFinished ), lengthTmp );
                if (!read.IsSuccess) return OperateResult.CreateFailedResult<byte[]>( read );

                lists.AddRange( read.Content );
                alreadyFinished += lengthTmp;
            }
            return OperateResult.CreateSuccessResult( lists.ToArray( ) );
        }
        
        #endregion

        #region Write One Register
        
        /// <summary>
        /// 写一个寄存器数据
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="high">高位</param>
        /// <param name="low">地位</param>
        /// <returns>返回写入结果</returns>
        public OperateResult WriteOneRegister( string address, byte high, byte low )
        {
            OperateResult<byte[]> command = BuildWriteOneRegisterCommand( address, new byte[] { high, low } );
            if (!command.IsSuccess) return command;

            return CheckModbusTcpResponse( command.Content );
        }

        /// <summary>
        /// 写一个寄存器数据
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">写入值</param>
        /// <returns>返回写入结果</returns>
        public OperateResult WriteOneRegister( string address, short value )
        {
            byte[] buffer = BitConverter.GetBytes( value );
            return WriteOneRegister( address, buffer[1], buffer[0] );
        }

        /// <summary>
        /// 写一个寄存器数据
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">写入值</param>
        /// <returns>返回写入结果</returns>
        public OperateResult WriteOneRegister( string address, ushort value )
        {
            byte[] buffer = BitConverter.GetBytes( value );
            return WriteOneRegister( address, buffer[1], buffer[0] );
        }
        
        #endregion

        #region Write Base
        
        /// <summary>
        /// 将数据写入到Modbus的寄存器上去，需要指定起始地址和数据内容
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <param name="value">写入的数据，长度根据data的长度来指示</param>
        /// <returns>返回写入结果</returns>
        /// <remarks>
        /// 富地址格式，支持携带站号信息，功能码信息，具体参照类的示例代码
        /// </remarks>
        /// <example>
        /// 此处演示批量写入的示例
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\Modbus\Modbus.cs" region="WriteExample2" title="Write示例" />
        /// </example>
        public override OperateResult Write( string address, byte[] value )
        {
            // 解析指令
            OperateResult<byte[]> command = BuildWriteRegisterCommand( address, value );
            if (!command.IsSuccess) return command;

            // 核心交互
            return CheckModbusTcpResponse( command.Content );
        }
        
        #endregion

        #region Write Coil

        /// <summary>
        /// 写一个线圈信息，指定是否通断
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">写入值</param>
        /// <returns>返回写入结果</returns>
        public OperateResult WriteCoil( string address, bool value )
        {
            // 解析指令
            OperateResult<byte[]> command = BuildWriteOneCoilCommand( address, value );
            if (!command.IsSuccess) return command;

            // 核心交互
            return CheckModbusTcpResponse( command.Content );
        }

        /// <summary>
        /// 批量写入线圈信息，指定是否通断
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="values">写入值</param>
        /// <returns>返回写入结果</returns>
        public OperateResult WriteCoil( string address, bool[] values )
        {
            // 解析指令
            OperateResult<byte[]> command = BuildWriteCoilCommand( address, values );
            if (!command.IsSuccess)  return command;

            // 核心交互
            return CheckModbusTcpResponse( command.Content );
        }
        
        #endregion

        #region Write bool[]

        /// <summary>
        /// 向寄存器中写入bool数组，返回值说明，比如你写入M100,那么data[0]对应M100.0
        /// </summary>
        /// <param name="address">要写入的数据地址</param>
        /// <param name="values">要写入的实际数据，长度为8的倍数</param>
        /// <returns>返回写入结果</returns>
        public OperateResult Write( string address, bool[] values )
        {
            return Write( address, BasicFramework.SoftBasic.BoolArrayToByte( values ) );
        }
        
        #endregion
        
        #region Object Override

        /// <summary>
        /// 返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串信息</returns>
        public override string ToString( )
        {
            return "ModbusRtu";
        }

        #endregion
        
    }
}
