/*
 * 
 * Date Created: 22.08.2022
 * Author: Lewis Comstive
 *
 */

/*
 * 
 * Changelog:
 *	Lewis - Initial creation
 *
 */

namespace HotF.Abilities
{
	public abstract class ToggleableAbility : Ability
	{
		/// <summary>
		/// When true, the ability can be toggled on or off.
		/// Typically this value is set to false while an ability
		/// is currently in the process of being activated or de-activated.
		/// </summary>
		protected virtual bool CanToggle { get; set; } = true;

		protected override bool OnActivated(bool activate)
		{
			if (!CanToggle)
				return false;

			bool result = OnActiveStateChanged();
			if (result)
				IsActive = activate;
			return result;
		}

		protected virtual bool OnActiveStateChanged() => true;
	}
}