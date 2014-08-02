﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace devTool
{
    public sealed class ConnectionDataEventArgs : ConnectionEventArgs
    {
        public IEnumerable<byte> Data { get; private set; }

        public ConnectionDataEventArgs(Connection connection, IEnumerable<byte> data)
            : base(connection)
        {
            this.Data = data ?? new byte[0];
        }

        public override string ToString()
        {
            return Connection.RemoteEndPoint != null
                ? string.Format("{0}: {1} bytes", Connection.RemoteEndPoint, Data.Count())
                : string.Format("Not Connected: {0} bytes", Data.Count());
        }
    }
}
