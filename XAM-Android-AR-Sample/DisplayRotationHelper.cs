using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware.Display;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XAM_Android_AR_Sample
{
    /// <summary>
    /// 
    /// </summary>
    /// Der DisplayManager dient zum identifizieren von sekundären Displays, die zur darstellung geeignet sind. 
    /// Anwendungen können ihren Inhalt automatisch auf Präsentationsdisplays projizieren (wie MirrorSharing)
    public class DisplayRotationHelper : Java.Lang.Object, DisplayManager.IDisplayListener
    {
        #region Fields
        private bool _mViewportChanged;
        private int _mViewPortWidth;
        private int _mViewPortHeigth;
        private Context _mContext;
        private Display _mDisplay;
        #endregion

        #region Propertys

        #endregion

        #region Constructor
        /// <summary>
        /// Constructs the DisplayRotationHelper but does not register the listener yet.
        /// </summary>
        /// <param name="context">Link to Context</param>
        public DisplayRotationHelper(Context context)
        {
            _mContext = context;
            _mDisplay = context.GetSystemService(Java.Lang.Class.FromType(typeof(IWindowManager)))
                .JavaCast<IWindowManager>().DefaultDisplay;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Registers the display listener. Should be called from {@link Activity#onResume()}
        /// </summary>
        public void OnResume()
        {
            _mContext.GetSystemService(Java.Lang.Class.FromType(typeof(DisplayManager)))
                .JavaCast<DisplayManager>().RegisterDisplayListener(this, null);
        }

        /// <summary>
        /// Unregisters the display listener. Should be called from {@link Activity#onPause()}.
        /// </summary>
        public void OnPause()
        {
           _mContext.GetSystemService(Java.Lang.Class.FromType(typeof(DisplayManager)))
                .JavaCast<DisplayManager>().UnregisterDisplayListener(this);
        }

        /// <summary>
        /// Records a change in surface dimensions. This will be later used by {@link #updateSessionIfNeeded(Session)}. Should be called from
        ///{@link android.opengl.GLSurfaceView.Renderer
        ///nSurfaceChanged(javax.microedition.khronos.opengles.GL10, int, int)}.
        /// </summary>
        /// <param name="width">the updated width of the surface</param>
        /// <param name="heigth">the updated height of the surface</param>
        public void OnSurfaceChanged(int width, int heigth)
        {
            _mViewPortWidth = width;
            _mViewPortHeigth = heigth;
            _mViewportChanged = true;

        }

        public void UpdateSessionIfNeeded()
        {

        }



        #region IDisplayListener implementierungen
        public void OnDisplayAdded(int displayId)
        {
           
        }

        public void OnDisplayChanged(int displayId)
        {
            throw new NotImplementedException();
        }

        public void OnDisplayRemoved(int displayId)
        {

        }
        #endregion


        #endregion

    }
}