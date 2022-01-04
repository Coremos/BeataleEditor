namespace Editor
{
    namespace Button
    {
        interface IButton
        {
            void OnClick();
        }

        abstract class MenuButton : IButton
        {
            public void OnClick() { }

            public virtual void OnMouseUp() { }
            public virtual void OnMouseDown() { }
            public virtual void OnMouseMove() { }
        }
    }
}