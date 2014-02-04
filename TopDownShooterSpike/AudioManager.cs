using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using OpenTK.Graphics.OpenGL;

namespace TopDownShooterSpike
{
    public class AudioManager
    {
        private SoundEffectInstance _backgroundSong; // the current background song
        private SoundEffectInstance _fadeInSong;     // the new song that's about to fade in once the current background song fades out

        private float _currentFadeTicks; // ticks until new song is faded in
        public int FadeTicks; // ticks it takes to fade in/out in seconds
        private bool _isFading; // set to true when you are fading in new song

        private static readonly ContentManager _manager = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get { return _instance ?? (_instance = new AudioManager()); }
        }

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

            _fadeInSong = _manager.Load<SoundEffect>("sfx/" + name).CreateInstance();
            _fadeInSong.Volume = 0;
            _fadeInSong.IsLooped = isLooped;
            _fadeInSong.Play();
            _currentFadeTicks = FadeTicks;
            _isFading = true;
        }

        /// <summary>
        /// Play a background song
        /// </summary>
        /// <param name="name"></param>
        public void PlayBackgroundMusic(string name, bool isLooped = true)
        {
            _backgroundSong = _manager.Load<SoundEffect>("sfx/" + name).CreateInstance();
            _backgroundSong.Volume = 100;
            _backgroundSong.IsLooped = isLooped;
            _backgroundSong.Play();
            _isFading = false;
        }

        /// <summary>
        /// Fade out any currently playing background music
        /// </summary>
        public void FadeOutBackgroundMusic()
        {
            _fadeInSong = null;
            _isFading = true;
        }

        public void Update(GameTime gameTime)
        {
            if (_isFading)
            {
                if (_currentFadeTicks == 0 && _fadeInSong != null)
                {
                    _fadeInSong.Dispose();
                    _isFading = false;
                }
                else
                {
                    _currentFadeTicks -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _currentFadeTicks = _currentFadeTicks < 0 ? 0 : _currentFadeTicks;

                    if(_backgroundSong != null)
                        _backgroundSong.Volume = _currentFadeTicks / (FadeTicks * 1.0f);

                    if(_fadeInSong != null)
                        _fadeInSong.Volume = 100 - (_currentFadeTicks / (FadeTicks * 1.0f));
                }
            }
        }

        public SoundEffectInstance Copy(ref SoundEffectInstance t)
        {
            return t;
        }
    }
}