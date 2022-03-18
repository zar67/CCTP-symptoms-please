namespace SymptomsPlease.UI.ToolTip
{
    /// <summary>
    /// Interface for designating a component that will show a tool tip when hovered over by the mouse.
    /// </summary>
    public interface IToolTipObject
    {
        /// <summary>
        /// Gets the data to display on the tool tip when raycasted.
        /// </summary>
        /// <returns><see cref="ToolTipData"/> to display.</returns>
        ToolTipData GetData();

        /// <summary>
        /// Handles the raycast and determines if the object is in a raycastable state.
        /// </summary>
        /// <returns>
        /// <para>true: Object is in a state to show tool tip information.</para>
        /// <para>false: tool tip information should not be shown.</para>
        /// </returns>
        bool HandleRaycast();
    }
}