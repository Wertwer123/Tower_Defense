using UnityEngine;

namespace PropertyAttributes
{
    public class OnValueChanged : PropertyAttribute
    {
       public string CallbackFunctionName { get; private set; }

       public OnValueChanged(string callbackFunctionName)
       {
           CallbackFunctionName = callbackFunctionName;
       }
    }
}