using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace S01_LoadModelWithBasicEffect
{
    public class Game1 : Game
    {
        // common game properties
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // display properties
        private float _aspectRatio;

        // camera
        private Vector3 _cameraPosition = new Vector3(0.0f, 0.0f, 3000.0f);
        private Vector3 _cameraTarget = Vector3.Zero;
        private Vector3 _cameraUpDirection = Vector3.Up;

        // first model to be displayed
        private const string Model1AssetName = "Lowpoly_Helicopter";
        private Model _model1;
        private Matrix _modelPosition = Matrix.Identity;
        private Vector3 _modelRotation = Vector3.Zero;

        // TODO: second model submarine

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // hint: Initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _model1 = Content.Load<Model>(Model1AssetName);

            _aspectRatio = _graphics.GraphicsDevice.Viewport.Width / (float) _graphics.GraphicsDevice.Viewport.Height;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _cameraPosition.Z -= 100;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _cameraPosition.Z += 100;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _modelRotation.Y += MathHelper.ToRadians(1);
                _modelRotation.Y %= MathHelper.ToRadians(360);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _modelRotation.Y -= MathHelper.ToRadians(1);
                _modelRotation.Y %= MathHelper.ToRadians(360);
            }

            // hint: Update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawModel();

            base.Draw(gameTime);
        }

        private void DrawModel()
        {
            var transforms = new Matrix[_model1.Bones.Count];
            _model1.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var mesh in _model1.Meshes)
            {
                foreach (var effect in mesh.Effects)
                {
                    var basicEffect = (BasicEffect) effect;

                    basicEffect.EnableDefaultLighting();
                    basicEffect.TextureEnabled = true;
                    //basicEffect.PreferPerPixelLighting = true;

                    basicEffect.World = transforms[mesh.ParentBone.Index] * _modelPosition;
                    basicEffect.World *= Matrix.CreateRotationX(_modelRotation.X);
                    basicEffect.World *= Matrix.CreateRotationY(_modelRotation.Y);
                    basicEffect.World *= Matrix.CreateRotationZ(_modelRotation.Z);

                    basicEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, _cameraUpDirection);

                    basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), _aspectRatio, 1.0f, 10000.0f);
                }

                mesh.Draw();
            }
        }
    }
}
