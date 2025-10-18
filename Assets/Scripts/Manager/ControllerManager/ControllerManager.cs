using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

public class ControllerManager : ManagerBase, IInitRootBefore
{
  [Inject]
  private DiContainer DiContainer { get; set; }
  [Inject]
  private IMessageManager MessageManager { get; set; }
  
  protected override IEnumerator OnInit()
  {
    string controllerName = GameConst.AssemblyNameForController;
    using (List<Type>.Enumerator enumerator = ((IEnumerable<Type>) ((IEnumerable<Assembly>) 
             AppDomain.CurrentDomain.GetAssemblies()).First<Assembly>((Func<Assembly, bool>) 
             (a => a.GetName().Name == controllerName)).GetTypes())
           .Where<Type>((Func<Type, bool>) (type => ((IEnumerable<Type>) type.GetInterfaces())
             .Any<Type>((Func<Type, bool>)(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (IController<>))) && !type.IsAbstract && !type.IsInterface)).ToList<Type>().GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        Type controllerType = enumerator.Current;
        Type messageType = ((IEnumerable<Type>) controllerType.GetInterfaces()).Where<Type>((Func<Type, bool>) 
          (t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (IController<>))).
          Select<Type, Type>((Func<Type, Type>) (t => ((IEnumerable<Type>) t.GetGenericArguments())
            .FirstOrDefault<Type>())).FirstOrDefault<Type>();
        MethodInfo handleMethod = controllerType.GetMethod("Handle");
        if (!(handleMethod == (MethodInfo) null) && !(messageType == (Type) null))
        {
          this.DiContainer.Bind(controllerType).AsTransient();
          this.MessageManager.RegisterController<MessageModel>(messageType, new Action<MessageModel>(Action));
        }

        void Action(MessageModel msg)
        {
          object obj = this.DiContainer.Resolve(controllerType);
          handleMethod.Invoke(obj, new object[1]
          {
            (object) msg
          });
        }
      }
      yield break;
    }
  }
}
