using System;

namespace HueCLI.Logic
{
    public class BridgeLightHTTPStatusCodeException : Exception
    {
        public BridgeLightHTTPStatusCodeException()
        : base()
        {
        }
    }

    public class BridgeDiscoveryHTTPStatusCodeException : Exception
    {
        public BridgeDiscoveryHTTPStatusCodeException()
        : base()
        {
        }
    }

    public class BridgeLinkHTTPStatusCodeException : Exception
    {
        public BridgeLinkHTTPStatusCodeException()
        : base()
        {
        } 
    }

    public class BridgeLinkButtonNotPressedException : Exception
    {
        public BridgeLinkButtonNotPressedException()
        : base()
        {
        } 
    }

    public class BridgeLightUnknownException : Exception
    {
        public BridgeLightUnknownException()
        : base()
        {
        }
    }

    public class BridgeLinkUnknownException : Exception
    {
        public BridgeLinkUnknownException()
        : base()
        {
        }
        
        public BridgeLinkUnknownException(string message)
        : base(message)
        {
        }
    }
}