using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace MonogamePieChartSample;
public class GameRoot : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _circleTexture;
    private Effect _pieEffect;
    private EffectParameter _region1Angle;
    private EffectParameter _region1;
    private EffectParameter _region2;
    public GameRoot()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferredBackBufferWidth = 1280;
    }

    protected override void Initialize()
    {
        const int Radius = 256;
        const int Diameter = Radius * 2;

        _circleTexture = new Texture2D(GraphicsDevice, Diameter, Diameter);
        _circleTexture.SetData(Enumerable.Range(0, Diameter * Diameter).Select(index =>
        {
            int y = index / Diameter;
            int x = index % Diameter;
            return Vector2.DistanceSquared(new Vector2(Radius), new Vector2(x, y)) < Radius * Radius ? Color.White : Color.Transparent;
        }).ToArray());

        _pieEffect = Content.Load<Effect>("chart");

        _region1Angle = _pieEffect.Parameters["region1Angle"];
        _region2 = _pieEffect.Parameters["region2"];
        _region1 = _pieEffect.Parameters["region1"];

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.Initialize();

        _region1.SetValue(Color.Red.ToVector4());
        _region2.SetValue(Color.Blue.ToVector4());
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _region1Angle.SetValue((Mouse.GetState().X / 1280f) * MathHelper.TwoPi - MathHelper.Pi);


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(effect: _pieEffect);
        _spriteBatch.Draw(_circleTexture, Window.ClientBounds.Size.ToVector2() / 2, null, Color.White, 0, _circleTexture.Bounds.Size.ToVector2() / 2, 1, SpriteEffects.None, 0);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
