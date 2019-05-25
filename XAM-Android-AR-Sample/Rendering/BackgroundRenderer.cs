using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.AR.Core;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio;
using Android.Opengl;

namespace XAM_Android_AR_Sample.Rendering
{
    public class BackgroundRenderer
    {
        #region Fields
        private const string TAG = "BACKGROUNDRENDERER";
        private const int COORDS_PER_VERTEX = 3;
        private const int TEXCOORDS_PER_VERTEX = 2;
        private const int FLOAT_SIZE = 4;

        private FloatBuffer mQuadVertices;
        private FloatBuffer mQuadTexCoord;
        private FloatBuffer mQuadTexCoordTransformed;

        private int mQuadProgram;
        private int mQuadPositionParam;
        private int mQuadTexCoordParam;

        private int mTextureTarget = GLES11Ext.GlTextureExternalOes;

        static readonly float[] QUAD_COORDS = new float[]{
            -1.0f, -1.0f, 0.0f,
            -1.0f, +1.0f, 0.0f,
            +1.0f, -1.0f, 0.0f,
            +1.0f, +1.0f, 0.0f,
        };

        static readonly float[] QUAD_TEXCOORDS = new float[]{
            0.0f, 1.0f,
            0.0f, 0.0f,
            1.0f, 1.0f,
            1.0f, 0.0f,
        };
        #endregion

        #region Propertys
        public int TextureId
        {
            get;
            private set;
        } = -1;
        #endregion

        public BackgroundRenderer()
        {

        }

        #region Methods
        /**
		 * Allocates and initializes OpenGL resources needed by the background renderer.  Must be
		 * called on the OpenGL thread, typically in
		 * {@link GLSurfaceView.Renderer#onSurfaceCreated(GL10, EGLConfig)}.
		 *
		 * @param context Needed to access shader source.
		 */
        public void CreateOnGlThread(Context context)
        {
            // Generate the background texture.
            var textures = new int[1];

            //GLES20 = OpenGL ES 2.0  
            //Gibt eine bestimmte Anzahl an freien Texturnamen zurück. 
            GLES20.GlGenTextures(1, textures, 0);

            TextureId = textures[0];
            GLES20.GlBindTexture(mTextureTarget, TextureId);
            GLES20.GlTexParameteri(mTextureTarget, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
            GLES20.GlTexParameteri(mTextureTarget, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);
            GLES20.GlTexParameteri(mTextureTarget, GLES20.GlTextureMinFilter, GLES20.GlNearest);
            GLES20.GlTexParameteri(mTextureTarget, GLES20.GlTextureMagFilter, GLES20.GlNearest);

            int numVertices = 4;
            if (numVertices != QUAD_COORDS.Length / COORDS_PER_VERTEX)
                throw new Exception("Unexpected number of vertices in BackgroundRenderer.");

            var bbVertices = ByteBuffer.AllocateDirect(QUAD_COORDS.Length * FLOAT_SIZE);
            bbVertices.Order(ByteOrder.NativeOrder());
            mQuadVertices = bbVertices.AsFloatBuffer();
            mQuadVertices.Put(QUAD_COORDS);
            mQuadVertices.Position(0);

            var bbTexCoords = ByteBuffer.AllocateDirect(numVertices * TEXCOORDS_PER_VERTEX * FLOAT_SIZE);
            bbTexCoords.Order(ByteOrder.NativeOrder());
            mQuadTexCoord = bbTexCoords.AsFloatBuffer();
            mQuadTexCoord.Put(QUAD_TEXCOORDS);
            mQuadTexCoord.Position(0);

            var bbTexCoordsTransformed = ByteBuffer.AllocateDirect(numVertices * TEXCOORDS_PER_VERTEX * FLOAT_SIZE);
            bbTexCoordsTransformed.Order(ByteOrder.NativeOrder());
            mQuadTexCoordTransformed = bbTexCoordsTransformed.AsFloatBuffer();

            int vertexShader = ShaderUtil.LoadGLShader(TAG, context,
                    GLES20.GlVertexShader, Resource.Raw.screenquad_vertex);
            int fragmentShader = ShaderUtil.LoadGLShader(TAG, context,
                    GLES20.GlFragmentShader, Resource.Raw.screenquad_fragment_oes);

            mQuadProgram = GLES20.GlCreateProgram();
            GLES20.GlAttachShader(mQuadProgram, vertexShader);
            GLES20.GlAttachShader(mQuadProgram, fragmentShader);
            GLES20.GlLinkProgram(mQuadProgram);
            GLES20.GlUseProgram(mQuadProgram);

            ShaderUtil.CheckGLError(TAG, "Program creation");

            mQuadPositionParam = GLES20.GlGetAttribLocation(mQuadProgram, "a_Position");
            mQuadTexCoordParam = GLES20.GlGetAttribLocation(mQuadProgram, "a_TexCoord");

            ShaderUtil.CheckGLError(TAG, "Program parameters");
        }
        #endregion
    }
}