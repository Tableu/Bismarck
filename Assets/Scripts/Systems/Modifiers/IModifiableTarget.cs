using System;

namespace Systems.Modifiers
{
    public interface IModifiableTarget
    {
        public void AttachModifer(Modifer modifer);
        public void RemoveModifer(Modifer modifer);
    }
}