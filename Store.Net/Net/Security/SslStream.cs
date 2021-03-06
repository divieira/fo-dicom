﻿// Copyright (c) 2010-2013 Anders Gustafsson, Cureos AB.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	public delegate bool RemoteCertificateValidationCallback(
		object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);
	
	public class SslStream : MemoryStream
	{
		#region FIELDS

		private readonly TcpClient.Stream _innerStream;
		private readonly bool _leaveInnerStreamOpen;
		private readonly RemoteCertificateValidationCallback _validateServerCertificate;
		
		#endregion

		#region CONSTRUCTORS

		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback validateServerCertificate)
		{
			var tcpClientStream = innerStream as TcpClient.Stream;
			if (tcpClientStream == null)
				throw new ArgumentException("Stream type not associated with TCP client", "innerStream");

			_innerStream = tcpClientStream;
			_leaveInnerStreamOpen = leaveInnerStreamOpen;
			_validateServerCertificate = validateServerCertificate;
		}

		public SslStream(Stream innerStream, bool leaveInnerStreamOpen) : this(innerStream, leaveInnerStreamOpen, null)
		{
		}

		public SslStream(Stream innerStream) : this(innerStream, true)
		{
		}

		#endregion

		#region METHODS

		public void AuthenticateAsServer(X509Certificate serverCertificate, 
			bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			throw new NotImplementedException("SSL server support is not implemented");
		}

		public void AuthenticateAsClient(string targetHost)
		{
			_innerStream.UpgradeToSsl(targetHost);
		}
		
		#endregion
	}
}