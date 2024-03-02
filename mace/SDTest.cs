using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

using BSI.MACE;
using BSI.MACE.AI;
using BSI.SimulationLibrary;
using BSI.SimulationLibrary.Formulas;
using BSI.MACE.PlugInNS;
using BSI.MACE.Network.DIS;

namespace CodeScriptPlugIn_NS
{
    /// <summary>
    /// Unique class holding execute method.
    /// </summary>
    public class SDTest
    {
        /* 

          UNCOMMENT THIS BLOCK IF YOU WANT TO TRIGGER YOUR
          CODESCRIPT VIA A BUTTON ADDED TO THE MACE UI

        /// <summary>
        /// Ribbon location to place icon;
        /// Can be any of the main MACE ribbon categories.
        /// Note that if left as an empty string or not defined, no button will be created in MACE
        /// and the Execute() function will be called immediately when the codescript
        /// is run/initialized.
        /// </summary>
        public static string RibbonName = "Mission Controls";

        /// <summary>
        /// Tooltip to show when over the button icon/text
        /// </summary>
        public static string ToolTip = "A helpful but concise description of what will happen when the button is clicked.";

        /// <summary>
        /// Name displayed on button.
        /// </summary>
        public static string ButtonName = "Button Name";
        //*/

        /// <summary>
        /// Method called when the codescript object is initialized. Any timers or 
        /// event handlers should be created here.
        /// </summary> 
        public static void Initialize(IMACEPlugInHost host)
        {
            // YOUR CODE HERE
        }
 
        /// <summary>
        /// Method called when a codescript is run, if no button is defined for the code script,
        /// or called when the button defined by the codescript is clicked. 
        /// </summary>
        /// <param name="host">The MACE plug in system interface.</param>
        public static void Execute(IMACEPlugInHost host)
        {
			// Global State Information
			string specifiedXml = "";
			var missionFilepath = "C:\\Users\\Public\\Public Documents\\MACE\\missions\\" + specifiedXml;
			bool missionLoaded = false;

			var physicalEntities = host.Mission.PhysicalEntities;
			Debug.WriteLine(physicalEntities);

			// Load a mission
			//missionLoaded = host.Mission.MissionCommands.NewMission();
			//missionLoaded = host.Mission.MissionCommands.LoadMission(missionFilepath);
			int i = 0;
			while (i < 25) {
				// Spawn an entity
				IMissionCommands.ModelAndTypeStructure modelAndType = new IMissionCommands.ModelAndTypeStructure() { Model = IMissionCommands.ModelAndTypeStructure.ModelTypes.Platform, Type = "B-2A" };
				IPhysicalEntity physicalEntity = host.Mission.MissionCommands.CreateEntity(modelAndType, new GeoPoint(32.0, -78.18, 1000.0), "ACE3");

				// Save the mission
				//host.Mission.MissionCommands.SaveMission(missionFilepath);

				// Delete Entity
				//host.Mission.MissionCommands.RemoveEntity(physicalEntity);

				// Perform Action
				//var parameters = "22.0,-81.1,100";
				//host.Mission.ActionRequest(disMACEenumerations.RallyTo, parameters, physicalEntity);
				var parameters = "22.0,-81.1,1,10,26.0,-81.1";
				host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
				i++;
			}
        }

        /// <summary>
        /// Method called when the codescript object is unloaded from MACE.
        /// Stop timers, unsubscribe to event handlers, and 
        /// otherwise clean up references here.
        /// </summary>
        public static void Close()
        {
            // YOUR CODE HERE
        }
   }
}



















