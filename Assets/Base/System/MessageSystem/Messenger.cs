using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base.MessageSystem
{
    public enum SystemMessage {Input = 0, Setting}
    public static class Messenger
    {
        private static Dictionary<Enum, List<Delegate>> _message = new Dictionary<Enum, List<Delegate>>();

        #region Adding listener

        /// <summary>
        /// Register message handler for event with "eventNumber" id
        /// </summary>
        /// <param name="eventNumber"> the id of the message </param>
        /// <param name="handler">the handler of the message with 0 param</param>
        public static void RegisterListener(Enum eventNumber, Callback handler)
        {
            if (!_message.ContainsKey(eventNumber))
            {
                _message.Add(eventNumber, new List<Delegate>() {handler});
            }
            else
            {
                _message[eventNumber].Add(handler);
            }
        }

        /// <summary>
        /// Register message handler for event with "eventNumber" id
        /// </summary>
        /// <param name="eventNumber"> the id of the message </param>
        /// <param name="handler">the handler of the message with 1 param</param>
        public static void RegisterListener<T>(Enum eventNumber, Callback<T> handler)
        {
            if (!_message.ContainsKey(eventNumber))
            {
                _message.Add(eventNumber, new List<Delegate>() {handler});
            }
            else
            {
                _message[eventNumber].Add(handler);
            }
        }

        /// <summary>
        /// Register message handler for event with "eventNumber" id
        /// </summary>
        /// <param name="eventNumber"> the id of the message </param>
        /// <param name="handler">the handler of the message with 2 param</param>
        public static void RegisterListener<T, U>(Enum eventNumber, Callback<T, U> handler)
        {
            if (!_message.ContainsKey(eventNumber))
            {
                _message.Add(eventNumber, new List<Delegate>() {handler});
            }
            else
            {
                _message[eventNumber].Add(handler);
            }
        }

        /// <summary>
        /// Register message handler for event with "eventNumber" id
        /// </summary>
        /// <param name="eventNumber"> the id of the message </param>
        /// <param name="handler">the handler of the message with 3 param</param>
        public static void RegisterListener<T, U, V>(Enum eventNumber, Callback<T, U, V> handler)
        {
            if (!_message.ContainsKey(eventNumber))
            {
                _message.Add(eventNumber, new List<Delegate>() {handler});
            }
            else
            {
                _message[eventNumber].Add(handler);
            }
        }

        /// <summary>
        /// Register message handler for event with "eventNumber" id
        /// </summary>
        /// <param name="eventNumber"> the id of the message </param>
        /// <param name="handler">the handler of the message with 4 param</param>
        public static void RegisterListener<T, U, V, X>(Enum eventNumber, Callback<T, U, V, X> handler)
        {
            if (!_message.ContainsKey(eventNumber))
            {
                _message.Add(eventNumber, new List<Delegate>() {handler});
            }
            else
            {
                _message[eventNumber].Add(handler);
            }
        }

        #endregion

        #region Remove listener

        /// <summary>
        /// Remove the handler from message
        /// </summary>
        /// <param name="messageId"> the message id </param>
        /// <param name="handler"> the handler of the message </param>
        public static void RemoveListener(Enum messageId, Callback handler)
        {
            if (OnRemoveListener(messageId, handler))
            {
                _message[messageId].Remove(handler);
            }
            else
            {
                Debug.LogWarning("Error when remove listener");
            }
        }

        /// <summary>
        /// Remove the handler from message
        /// </summary>
        /// <param name="messageId"> the message id </param>
        /// <param name="handler"> the handler of the message </param>
        public static void RemoveListener<T>(Enum messageId, Callback<T> handler)
        {
            if (OnRemoveListener(messageId, handler))
            {
                _message[messageId].Remove(handler);
            }
            else
            {
                Debug.LogWarning("Error when remove listener");
            }
        }

        /// <summary>
        /// Remove the handler from message
        /// </summary>
        /// <param name="messageId"> the message id </param>
        /// <param name="handler"> the handler of the message </param>
        public static void RemoveListener<T, U>(Enum messageId, Callback<T, U> handler)
        {
            if (OnRemoveListener(messageId, handler))
            {
                _message[messageId].Remove(handler);
            }
            else
            {
                Debug.LogWarning("Error when remove listener");
            }
        }

        /// <summary>
        /// Remove the handler from message
        /// </summary>
        /// <param name="messageId"> the message id </param>
        /// <param name="handler"> the handler of the message </param>
        public static void RemoveListener<T, U, V>(Enum messageId, Callback<T, U, V> handler)
        {
            if (OnRemoveListener(messageId, handler))
            {
                _message[messageId].Remove(handler);
            }
            else
            {
                Debug.LogWarning("Error when remove listener");
            }
        }

        /// <summary>
        /// Remove the handler from message
        /// </summary>
        /// <param name="messageId"> the message id </param>
        /// <param name="handler"> the handler of the message </param>
        public static void RemoveListener<T, U, V, X>(Enum messageId, Callback<T, U, V, X> handler)
        {
            if (OnRemoveListener(messageId, handler))
            {
                _message[messageId].Remove(handler);
            }
            else
            {
                Debug.LogWarning("Error when remove listener");
            }
        }

        #endregion

        #region Exeption handling

        private static bool OnRemoveListener(Enum messageId, Delegate handler)
        {
            bool result1 = false, result2 = false, result3 = false;
            if (result1 = (_message.ContainsKey(messageId)))
            {
                if (result2 = _message[messageId].Count > 0)
                {
                    result3 = _message[messageId].Contains(handler);
                }
            }

            return result1 && result2 && result3;
        }

        private static bool OnRaiseMessage(Enum messageId)
        {
            return _message.ContainsKey(messageId) && _message[messageId].Count > 0;
        }

        #endregion

        #region Broadcast message

        public static void RaiseMessage(Enum id)
        {
            if (OnRaiseMessage(id))
            {
                foreach (var handler in _message[id])
                {
                    try
                    {
                        ((Callback) handler)?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        continue;
                    }
                }
            }
        }

        public static void RaiseMessage<T>(Enum id, T arg1)
        {
            if (OnRaiseMessage(id))
            {
                foreach (var handler in _message[id])
                {
                    try
                    {
                        ((Callback<T>) handler)?.Invoke(arg1);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        continue;
                    }
                }
            }
        }

        public static void RaiseMessage<T, U>(Enum id, T arg1, U arg2)
        {
            if (OnRaiseMessage(id))
            {
                foreach (var handler in _message[id])
                {
                    try
                    {
                        ((Callback<T, U>) handler)?.Invoke(arg1, arg2);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        continue;
                    }
                }
            }
        }

        public static void RaiseMessage<T, U, V>(Enum id, T arg1, U arg2, V arg3)
        {
            if (OnRaiseMessage(id))
            {
                foreach (var handler in _message[id])
                {
                    try
                    {
                        ((Callback<T, U, V>) handler)?.Invoke(arg1, arg2, arg3);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        continue;
                    }
                }
            }
        }

        public static void RaiseMessage<T, U, V, X>(Enum id, T arg1, U arg2, V arg3, X arg4)
        {
            if (OnRaiseMessage(id))
            {
                foreach (var handler in _message[id])
                {
                    try
                    {
                        ((Callback<T, U, V, X>) handler)?.Invoke(arg1, arg2, arg3, arg4);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        continue;
                    }
                }
            }
        }

        #endregion

        public static void CleanUp()
        {
            foreach (var message in _message)
            {
                message.Value.Clear();
            }

            _message.Clear();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RuntimeInit()
        {
            //SceneManager.sceneLoaded += SceneLoadedCallback;
        }
        
        static void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
        {
            // Clear event table every time scene changed
            CleanUp();
        }
    }
}