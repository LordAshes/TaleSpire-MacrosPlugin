using BepInEx;
using BepInEx.Configuration;
using System;
using UnityEngine;

namespace LordAshes
{
	[BepInPlugin(Guid, Name, Version)]
	[BepInDependency(LordAshes.FileAccessPlugin.Guid)]
	[BepInDependency(LordAshes.StatMessaging.Guid)]
	[BepInDependency(RadialUI.RadialUIPlugin.Guid)]
	public partial class MacrosPlugin : BaseUnityPlugin
	{
		// Plugin info
		public const string Name = "Macros Plug-In";
		public const string Guid = "org.lordashes.plugins.macrosplugin";
		public const string Version = "1.0.0.0";

		public bool macroMenu = false;
		public int macroMenuSlide = 0;

		public System.Random random = new System.Random();

		/// <summary>
		/// Function for initializing plugin
		/// This function is called once by TaleSpire
		/// </summary>
		void Awake()
		{
			UnityEngine.Debug.Log("Macros Plugin: Active.");

			// Utility.PostOnMainPage(this.GetType());
		}

		/// <summary>
		/// Function for determining if view mode has been toggled and, if so, activating or deactivating Character View mode.
		/// This function is called periodically by TaleSpire.
		/// </summary>
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Backslash))
			{
				Debug.Log("Macro Menu Open = " + macroMenu);
				macroMenu = !macroMenu;
			}
			if ((macroMenu) && (macroMenuSlide < 160)) { macroMenuSlide = macroMenuSlide + 4; }
			if ((!macroMenu) && (macroMenuSlide > 0)) { macroMenuSlide = macroMenuSlide - 4; }
		}

		void OnGUI()
		{
			if (macroMenuSlide > 0)
			{
				int offset = 50;
				//
				// Macro: Attack1
				//
				if (Macro_01_Available())
				{
					if (GUI.Button(new Rect(1920 - macroMenuSlide, offset, 160, 40), "Short Sword (+3)")) { Macro_01_Execute(); }
					offset = offset + 50;
				}
				//
				// Macro: Attack2
				//
				if (Macro_02_Available())
				{
					if (GUI.Button(new Rect(1920 - macroMenuSlide, offset, 160, 40), "Dagger (+3)")) { Macro_02_Execute(); }
					offset = offset + 50;
				}
				//
				// Macro: Attack3
				//
				if (Macro_03_Available())
				{
					if (GUI.Button(new Rect(1920 - macroMenuSlide, offset, 160, 40), "Toss (+5)")) { Macro_03_Execute(); }
					offset = offset + 50;
				}

			}
		}

		/* ----------------------------------------- Macro Functions ----------------------------------------- */

		public bool Macro_01_Available()
		{
			return (SelectedCreature() != null);


		}

		public void Macro_01_Execute()
		{
			Creature attacker = SelectedCreature();
			Creature target = RadialCreature();

			if (attacker != null && target != null)
			{
				int roll = Random(1, 20) + 3;
				if (roll >= target.Stat0.Value)
				{
					attacker.Speak("Attack" + roll + "\r\nHit!");
					int dmg = Random(1, 6) + 3;
					target.Speak("Sliced for\r\n" + dmg + " HP");
				}
				else
				{
					attacker.Speak("Attack " + roll + "\r\nMiss!");
					target.Speak("Missed me!");
				}
			}


		}
		public bool Macro_02_Available()
		{
			return (SelectedCreature() != null);


		}

		public void Macro_02_Execute()
		{
			Creature attacker = SelectedCreature();
			Creature target = RadialCreature();

			if (attacker != null && target != null)
			{
				int roll = Random(1, 20) + 3;
				if (roll >= target.Stat0.Value)
				{
					attacker.Speak("Attack" + roll + "\r\nHit!");
					int dmg = Random(1, 4) + 3;
					target.Speak("Stabbed for\r\n" + dmg + " HP");
				}
				else
				{
					attacker.Speak("Attack " + roll + "\r\nMiss!");
					target.Speak("Missed me!");
				}
			}


		}
		public bool Macro_03_Available()
		{
			if (SelectedCreature() == null) { return false; }
			if (!SelectedCreature().Name.Contains("Monkey")) { return false; }
			return true;


		}

		public void Macro_03_Execute()
		{
			Creature attacker = SelectedCreature();
			Creature target = RadialCreature();

			if (attacker != null && target != null)
			{
				int roll = Random(1, 20) + 5;
				if (roll >= target.Stat0.Value)
				{
					attacker.Speak("Attack" + roll + "\r\nHit!");
					int dmg = Random(1, 6) + Random(1, 6) + 3;
					target.Speak("Stabbed for\r\n" + dmg + " HP");
				}
				else
				{
					attacker.Speak("Attack " + roll + "\r\nMiss!");
					target.Speak("Missed me!");
				}
			}


		}


		/* ----------------------------------------- Helper Functions ----------------------------------------- */

		public CreatureBoardAsset SelectedAsset()
		{
			CreatureBoardAsset asset;
			CreaturePresenter.TryGetAsset(LocalClient.SelectedCreatureId, out asset);
			return asset;
		}

		public CreatureBoardAsset RadialAsset()
		{
			CreatureBoardAsset asset;
			CreaturePresenter.TryGetAsset(new CreatureGuid(RadialUI.RadialUIPlugin.GetLastRadialTargetCreature()), out asset);
			return asset;
		}

		public Creature SelectedCreature()
		{
			CreatureBoardAsset asset = SelectedAsset();
			return (asset != null) ? asset.Creature : null;
		}

		public Creature RadialCreature()
		{
			CreatureBoardAsset asset = RadialAsset();
			return (asset != null) ? asset.Creature : null;
		}

		public int Random(int min, int max)
		{
			return random.Next(min, max);
		}
	}
}
