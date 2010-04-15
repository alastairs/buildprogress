using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Drawing;

namespace BuildProgress
{
	/// <summary>
    /// The object for implementing an Add-in.
    /// </summary>
	/// <seealso class="IDTExtensibility2" />
	public class Connect : IDTExtensibility2
	{
        private DTE2 _applicationObject;
        private BuildEvents _buildEvents;
        private int _numberOfProjectsBuilt;
        private int _numberOfProjects;

        private bool _buildErrorDetected;

        #region Events

        /// <summary>
        /// Implements the OnConnection method of the IDTExtensibility2 
        /// interface. Receives notification that the Add-in is being loaded.
        /// </summary>
		/// <param name="application">Root object of the host application.</param>
        /// <param name="connectMode">Describes how the Add-in is being loaded.</param>
        /// <param name="addInInstance">Object representing this Add-in.</param>
        /// <param name="custom">Array of custom parameters</param>
		/// <seealso class="IDTExtensibility2" />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInstance, ref Array custom)
		{
			_applicationObject = (DTE2)application;

            _buildEvents = _applicationObject.Events.BuildEvents;
            _buildEvents.OnBuildBegin += OnBuildBegin;
            _buildEvents.OnBuildProjConfigDone += OnBuildProjConfigDone;
            _buildEvents.OnBuildDone += OnBuildDone;
		}

        /// <summary>
        /// Implements a handler for the OnBuildBegin build event.  Resets the 
        /// task bar to 0 (and associated build state metadata stored by the 
        /// add-in).  
        /// </summary>
        /// <param name="scope">The scope of the build.</param>
        /// <param name="action">The build action (e.g., Clean).</param>
		private void OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            InitialiseTaskBar();
            
            InitialiseProgressValues();

            UpdateProgressValueAndState(false);
        }

        /// <summary>
        /// Implements a handler for the OnBuildProjConfigDone build event.  
        /// Increments the task bar progress indicator, and turns it red on a 
        /// failed project build.
        /// </summary>
        /// <param name="project">The name of the project that has finished 
        /// building.</param>
        /// <param name="projectConfig">The configuration (debug, release, etc.)
        /// of the project that has finished building.</param>
        /// <param name="platform">The processor architecture (AnyCPU, etc.) of
        /// the project that has finished building.</param>
        /// <param name="solutionConfig">The solution configuration (as defined
        /// in the Configuration Manager) of the project that has finished 
        /// building.</param>
        /// <param name="success">A flag indicating whether or not the project
        /// built successfully.</param>
        private void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            UpdateProgressValueAndState(!success);
        }

        /// <summary>
        /// Sets the appropriate overlay icon to display whether the build was a
        /// success or failure. 
        /// </summary>
        /// <param name="scope">The scope of the build.</param>
        /// <param name="action">The build action (e.g., Clean).</param>
        private void OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            if (_buildErrorDetected)
            {
                TaskbarManager.Instance.SetOverlayIcon((Icon)Resources.ResourceManager.GetObject("cross"), "Build Failed");
            }
            else
            {
                TaskbarManager.Instance.SetOverlayIcon((Icon)Resources.ResourceManager.GetObject("tick"), "Build Succeeded");
            }

            // Add in a small delay so that the progress bar visibly reaches 100%
            System.Threading.Thread.Sleep(100);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Resets any existing build state to give a fresh start.
        /// </summary>
        private void InitialiseProgressValues()
        {
            _buildErrorDetected = false;
            _numberOfProjectsBuilt = 0;
            _numberOfProjects = _applicationObject.Solution.SolutionBuild.BuildDependencies.Count;
        }

        /// <summary>
        /// Reset the task bar progress indicator to 0 (no progress) and remove 
        /// any overlay icon displayed.
        /// </summary>
        private static void InitialiseTaskBar()
        {
            TaskbarManager.Instance.SetOverlayIcon(null, string.Empty);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        /// <summary>
        /// Increments the taskbar progress indicator by one step, and changes
        /// the taskbar state to "error" (i.e., red) if a build error has been
        /// reported.
        /// </summary>
        /// <param name="errorThrown"></param>
        private void UpdateProgressValueAndState(bool errorThrown)
        {
            if (_numberOfProjectsBuilt < 0)
                _numberOfProjectsBuilt = 0;

            if (_numberOfProjectsBuilt > _numberOfProjects)
                _numberOfProjectsBuilt = _numberOfProjects;

            // Maximum value is N-1, as we count the projects from 0.
            TaskbarManager.Instance.SetProgressValue(_numberOfProjectsBuilt, _numberOfProjects - 1);

            if (errorThrown)
            {
                _buildErrorDetected = true;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
            }

            _numberOfProjectsBuilt++;
        }

        #endregion

        #region Unimplemented methods of IDTExtensibility2

        /// <summary />
        /// <param name="custom"></param>
        public void OnAddInsUpdate(ref Array custom)
        {
            
        }

        /// <summary />
        /// <param name="custom"></param>
        public void OnBeginShutdown(ref Array custom)
        {
            
        }

        /// <summary />
        /// <param name="RemoveMode"></param>
        /// <param name="custom"></param>
        public void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
            
        }
        
        /// <summary />
        /// <param name="custom"></param>
        public void OnStartupComplete(ref Array custom)
        {
            
        }

        #endregion
    }
}