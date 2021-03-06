using System;
using A19.Messaging;
using A19.Messaging.Common;

namespace Mrh.Messaging.Json
{
    /// <summary>
    ///     Used to create envelops for sending messages.
    /// </summary>
    /// <typeparam name="TPayloadType">The type for the payload.</typeparam>
    public class JsonEnvelopeFactory<TPayloadType> : IEnvelopFactory<TPayloadType, string> where TPayloadType : struct
    {
        private readonly int maxFrameSize;

        public JsonEnvelopeFactory(
            IMessageSetting messageSetting)
        {
            this.maxFrameSize = messageSetting.MaxFrameSize;
        }

        public int CreateEnvelops(Message<TPayloadType, string> message,
            Action<MessageEnvelope<TPayloadType, string>> envelopHandler)
        {
            if (message.Body.Length <= maxFrameSize)
            {
                envelopHandler(new MessageEnvelope<TPayloadType, string>
                {
                    Body = message.Body,
                    Number = 0,
                    Total = 1,
                    ConnectionId = message.MessageIdentifier.ConnectionId,
                    CorrelationId = message.MessageIdentifier.CorrelationId,
                    MessageType = message.MessageType,
                    PayloadType = message.PayloadType,
                    UserId = message.UserId,
                    MessageResultType = message.MessageResultType,
                    TotalBodyLength = message.Body.Length,
                    ToConnectionId = message.ToConnectionId
                });
                return 1;
            }
            else
            {
                var total = this.Total(message);
                for (short i = 0; i < total; i++)
                {
                    envelopHandler(Create(
                        message,
                        i,
                        total));
                }

                return total;
            }
        }

        private int Total(Message<TPayloadType, string> message)
        {
            var total = message.Body.Length / this.maxFrameSize;
            if (message.Body.Length % this.maxFrameSize > 0)
            {
                total++;
            }

            return total;
        }

        public void CreateFragment(Message<TPayloadType, string> message, int frameNumber,
            Action<MessageEnvelope<TPayloadType, string>> envelopHandler)
        {
            envelopHandler(this.Create(
                message,
                frameNumber,
                Total(message)));
        }

        private MessageEnvelope<TPayloadType, string> Create(
            Message<TPayloadType, string> message,
            int frame,
            int total)
        {
            var start = CalculateStart(frame);
            var length = Math.Min(this.maxFrameSize, message.Body.Length - start);
            return new MessageEnvelope<TPayloadType, string>
            {
                Body = message.Body.Substring(start, length),
                Number = frame,
                Total = total,
                ConnectionId = message.ToConnectionId.Value,
                CorrelationId = message.MessageIdentifier.CorrelationId,
                MessageType = message.MessageType,
                PayloadType = message.PayloadType,
                UserId = message.UserId,
                MessageResultType = message.MessageResultType,
                TotalBodyLength = length
            };
        }

        private int CalculateStart(int frame)
        {
            return frame * this.maxFrameSize;
        }
    }
}