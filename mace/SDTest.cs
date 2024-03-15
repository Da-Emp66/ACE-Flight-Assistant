using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Web;
using System.Threading;
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
	public const int DEFAULT_ORBIT_RADIUS = 5;

	enum OrbitDirection
	{
		Left,
		Right,
		Unspecified
	}

	enum OrbitType
	{
		Circle,
		Polygon,
		Figure8,
		RaceTrack,
		Unspecified
	}

	struct Coordinate
	{
		public float latitude;
		public float longitude;
		public Coordinate(float lat, float lon)
		{
			latitude = lat;
			longitude = lon;
		}
		public override string ToString()
		{
			return "" + latitude + "," + longitude;
		}
	}

	// struct Weapon
	// {
	// 	kind;
	// 	domain;
	// 	country;
	// 	category;
	// 	subCategory;
	// 	specific;
	// 	extra;
	// }

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
            
        }
 
        /// <summary>
        /// Method called when a codescript is run, if no button is defined for the code script,
        /// or called when the button defined by the codescript is clicked. 
        /// </summary>
        /// <param name="host">The MACE plug in system interface.</param>
        public static void Execute(IMACEPlugInHost host)
        {

			/// Function Definitions

			// Manipulate Scenario (CRUD)
			IPhysicalEntity SpawnEntity(string callsign, string type, GeoPoint location)
			{
				IMissionCommands.ModelAndTypeStructure modelAndType = new IMissionCommands.ModelAndTypeStructure() { Model = IMissionCommands.ModelAndTypeStructure.ModelTypes.Platform, Type = type };
				IPhysicalEntity physicalEntity = host.Mission.MissionCommands.CreateEntity(modelAndType, location, callsign);
				return physicalEntity;
			}
			var physicalEntity = SpawnEntity("ACE0", "C-17A", new GeoPoint(28.0, -78.18, 1000.0));
			
			// Manipulate Entity Behaviors (All defined in MACE Network Interface)
			void FlyToLocation(IPhysicalEntity physicalEntity, Coordinate coordinate, OrbitDirection direction = OrbitDirection.Right, int radiusInNauticalMiles = DEFAULT_ORBIT_RADIUS, Coordinate coordinate2 = default, OrbitType orbitType = OrbitType.Unspecified)
			{
				if (orbitType == OrbitType.Unspecified)
				{
					var parameters = "" + coordinate + "," + (int)direction + "," + radiusInNauticalMiles;
					host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
				}
				else if (orbitType == OrbitType.Circle)
				{
					var parameters = "" + coordinate + "," + (int)direction + "," + radiusInNauticalMiles;
					host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
				}
				else if (orbitType == OrbitType.RaceTrack)
				{
					var parameters = "" + coordinate + "," + (int)direction + "," + radiusInNauticalMiles + "," + coordinate2;
					host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
				}
				else if (orbitType == OrbitType.Figure8)
				{
					var parameters = "" + coordinate + "," + (int)direction + "," + radiusInNauticalMiles + "," + coordinate2 + "," + (int)orbitType;
					host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
				}
				else if (orbitType == OrbitType.Polygon)
				{
					var parameters = "" + coordinate + "," + (int)direction + "," + radiusInNauticalMiles + "," + "," + "," + (int)orbitType;
					host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
				}
			}
			// void OldFlyToLocation(float latitude, float longitude, int speedInKnots)
			// {
			// 	var parameters = "" + latitude + "," + longitude + "," + speedInKnots;
			// 	host.Mission.ActionRequest(disMACEenumerations.RallyTo, parameters, physicalEntity);
			// }
			void Orbit(IPhysicalEntity physicalEntity)
			{
				// TODO: Get coordinate of the entity requested
				FlyToLocation(new Coordinate(latitude, longitude));
			}
			void ShootWeapon(IPhysicalEntity physicalEntity)
			{
				host.Mission.ActionRequest(disMACEenumerations.ShootWeapon, parameters, physicalEntity);
			}
			
			// Global State Information
			string specifiedXml = "";
			var missionFilepath = "C:\\Users\\Public\\Public Documents\\MACE\\missions\\" + specifiedXml;
			bool missionLoaded = false;
			var physicalEntities = host.Mission.PhysicalEntities;
			Console.WriteLine(physicalEntities);


			// Set up HTTP Server
			var alive = true;
			var port = 8280;
			var listener = new HttpListener();
			listener.Prefixes.Add("http://localhost:" + port.ToString() + "/");

			void Receive()
			{
				listener.BeginGetContext(new AsyncCallback(Listen), listener);
			}
			
			void Listen(IAsyncResult result)
			{
				if (listener.IsListening)
				{
					var context = listener.EndGetContext(result);
					var request = context.Request;
					
					// Delete Entity
					host.Mission.MissionCommands.RemoveEntity(physicalEntity);

					physicalEntity = SpawnEntity("ACEn", "C-17A", new GeoPoint(28.0, -78.18, 1000.0));

					var parameters = "32.36,-70.0,1000";
					host.Mission.ActionRequest(disMACEenumerations.RallyTo, parameters, physicalEntity);

					Console.WriteLine($"{request.HttpMethod} {request.Url}");
					if (request.HasEntityBody)
					{
						var body = request.InputStream;
						var encoding = request.ContentEncoding;
						var reader = new StreamReader(body, encoding);

						if (request.ContentType != null)
						{
							Console.WriteLine("Client data content type {0}", request.ContentType);
						}
						Console.WriteLine("Client data content length {0}", request.ContentLength64);

						Console.WriteLine("Start of data:");
						string s = reader.ReadToEnd();
						Console.WriteLine(s);
						Console.WriteLine("End of data:");
						reader.Close();
						body.Close();

						// TODO: Get the command from the body, do `switch cmd {...}`
					}

					var response = context.Response;
					response.StatusCode = (int)HttpStatusCode.OK;
					response.ContentType = "text/plain";
					response.OutputStream.Write(new byte[] { }, 0, 0);
					response.OutputStream.Close();
					
					Receive();
				}
			}

			void SpinUpHttpServer()
			{
				listener.Start();
				Receive();
				while (alive) {}
				listener.Stop();
			}

			// Spin up the HTTP server
			ThreadStart httpServer = SpinUpHttpServer;
			Thread httpThread = new Thread(httpServer);
			httpThread.Start();

			// Load a mission
			//missionLoaded = host.Mission.MissionCommands.NewMission();
			//missionLoaded = host.Mission.MissionCommands.LoadMission(missionFilepath);
			//int i = 0;
			//while (i < 25) {
	        
			// Save the mission
			//host.Mission.MissionCommands.SaveMission(missionFilepath);

			
			
			// Perform 
			//var parameters = "22.0,-81.1,1,10,26.0,-81.1";
			//host.Mission.ActionRequest(disMACEenumerations.StartLoiter, parameters, physicalEntity);
			//i++;
			//}
        }

        /// <summary>
        /// Method called when the codescript object is unloaded from MACE.
        /// Stop timers, unsubscribe to event handlers, and 
        /// otherwise clean up references here.
        /// </summary>
        public static void Close()
        {
            
        }
   }
}





























































