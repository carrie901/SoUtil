﻿using System;
using System.Collections.Generic;
using Framework.UI.Core;
using UnityEngine.UI;

namespace Framework.UI.Wrap
{
    public static class BindTool
    {
        private static readonly Dictionary<Type,Type> SupportWrapperTypes = new Dictionary<Type, Type>()
        {
            {typeof(Text) , typeof(TextWrapper)},
            {typeof(Toggle),typeof(ToggleWrapper)},
            {typeof(InputField),typeof(InputFieldWrapper)},
            {typeof(Slider),typeof(SliderWrapper)},
            {typeof(Button),typeof(ButtonWrapper)},
            {typeof(View), typeof(ViewWrapper)},
            {typeof(Image), typeof(ImageWrapper)},
            {typeof(Dropdown), typeof(DropdownWrapper)},
        };
        
        private static readonly object[] Args = new object[1];

        public static object GetDefaultBind<T>(T component) where T : class
        {
            foreach (var type in SupportWrapperTypes)
            {
                if (type.Key.IsInstanceOfType(component))
                {
                    Args[0] = component;
                    return Activator.CreateInstance(type.Value, Args);
                }
            }
            return component;
        }

    }
}
