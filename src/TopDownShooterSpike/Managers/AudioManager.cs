using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace TopDownShooterSpike.Managers
{
    public class AudioManager
    {
        #region Properties

        private List<SoundEffectInstance> _backgroundMusic = new List<SoundEffectInstance>(); // list of current background music - 0 is current 1 is the music being faded in
        private string _lastSong;

        private float _currentFadeTicks; // ticks until new song is faded in
        public int FadeTicks; // ticks it takes to fade in/out in seconds
        private bool _isFading; // set to true when you are fading in new song

        private static readonly ContentManager _manager = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get { return _instance ?? (_instance = new AudioManager()); }
        }

        #endregion

        #region Play

        /// <summary>
        /// Play one time sound effects
        /// </summary>
        /// <param name="name"></param>
        public void PlaySound(string name)
        {
            var b = _manager.Load<SoundEffect>("sfx/" + name);
            b.Play();
        }

        /// <summary>
        /// Fade in a looping background song
        /// </summary>
        /// <param name="name"></param>
        public void FadeInBackgroundMusic(string name, bool isLooped = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                FadeOutBackgroundMusic();
                return;
            }
            else if (_backgroundMusic.Count > 0 && _lastSong == name)
                return;

            _backgroundMusic.Add(_manager.Load<SoundEffect>("sfx/" + name).CreateInstance());
            var index = _backgroundMusic.Count - 1;

            _backgroundMusic[index].Volume = 0;
            _backgroundMusic[index].IsLooped = isLooped;
            _backgroundMusic[index].Play();
            _currentFadeTicks = FadeTicks;
            _isFading = true;
            _lastSong = name;
        }

        /// <summary>
        /// Play a background song
        /// </summary>
        /// <param name="name"></param>
        public void PlayBackgroundMusic(string name, bool isLooped = true)
        {
            _backgroundMusic.Clear();
            _backgroundMusic.Add(_manager.Load<SoundEffect>("sfx/" + name).CreateInstance());
            _backgroundMusic[0].Volume = 100;
            _backgroundMusic[0].IsLooped = isLooped;
            _backgroundMusic[0].Play();
            _isFading = false;
            _lastSong = name;
        }

        /// <summary>
        /// Fade out any currently playing background music
        /// </summary>
        public void FadeOutBackgroundMusic()
        {
            if(_backgroundMusic.Count > 1)
                _backgroundMusic.RemoveAt(1);

            _isFading = true;
            _lastSong = string.Empty;
        }

        #endregion

        #region Hooks

        /// <summary>
        /// Update the fading in and out of songs
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (_isFading)
            {
                if (_currentFadeTicks == 0 && _backgroundMusic.Count > 1)
                {
                    _backgroundMusic.RemoveAt(0);
                    _isFading = false;
                }
                else
                {
                    _currentFadeTicks -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _currentFadeTicks = _currentFadeTicks < 0 ? 0 : _currentFadeTicks;

                    if (_backgroundMusic.Count > 0)
                        _backgroundMusic[0].Volume = _currentFadeTicks / (FadeTicks * 1.0f);

                    if(_backgroundMusic.Count > 1)
                        _backgroundMusic[1].Volume = 100 - (_currentFadeTicks / (FadeTicks * 1.0f));
                }
            }
        }

        /// <summary>
        /// Removes everything from memory
        /// </summary>
        public void UnloadContent()
        {
            _backgroundMusic.Clear();
            _instance = null;
        }

        #endregion
    }
}