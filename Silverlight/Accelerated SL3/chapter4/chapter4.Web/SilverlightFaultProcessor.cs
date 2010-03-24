using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//added
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;



namespace chapter4.Web
{
    public class SilverlightFaultProcessor : IDispatchMessageInspector
    {
        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(
            ref System.ServiceModel.Channels.Message request,
            System.ServiceModel.IClientChannel channel,
            System.ServiceModel.InstanceContext instanceContext)
        {
            return (null);
        }

        public void BeforeSendReply(
          ref System.ServiceModel.Channels.Message reply,
          object correlationState)
        {
            if (reply.IsFault)
            {
                // If it's a fault, change the status code 
                // to 200 so Silverlight can receive the 
                // details of the fault

                HttpResponseMessageProperty responseProperty =
                   new HttpResponseMessageProperty();

                responseProperty.StatusCode =
                        System.Net.HttpStatusCode.OK;

                reply.Properties[HttpResponseMessageProperty.Name]
                      = responseProperty;
            }
        }

        #endregion
    }

}
