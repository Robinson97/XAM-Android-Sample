using System;
using Android.Content;
using Android.Hardware.Display;
using Android.Runtime;
using Android.Views;

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

        /// <summary>
        /// Updates the session display geometry if a change was posted either by
        /// {@link #onSurfaceChanged(int, int)} call or by {@link #onDisplayChanged(int)} system
        ///callback.This function should be called explicitly before each call to
        ///{@link Session#update()}. This function will also clear the 'pending update' (viewportChanged)flag.
        /// </summary>
        /// <param name="session">object to update if display geometry changed</param>
        public void UpdateSessionIfNeeded(Google.AR.Core.Session session)
        {
            if (_mViewportChanged)
            {
                int displayRotation = (int)_mDisplay.Rotation;
                session.SetDisplayGeometry(displayRotation, _mViewPortWidth, _mViewPortHeigth);
                _mViewportChanged = false;
            }
        }

        /// <summary>
        /// Returns the current rotation state of android display
        /// </summary>
        /// <returns>The current rotation state of android display</returns>
        public int GetLocation()
        {
            return (int)_mDisplay.Rotation;
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