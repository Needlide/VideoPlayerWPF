using System;
using System.Collections.Generic;

namespace VideoPlayerWPF
{
    internal class SourceController
    {
        #region Fields
        List<Uri> _sources = new List<Uri>();
        int _position = 0;
        #endregion

        #region Properties
        public List<Uri> Sources { get => _sources; set => _sources = value; }
        public int Position { get => _position; }
        #endregion

        #region Constructors
        public SourceController() { }
        #endregion

        #region Methods
        public Uri GetSource()
        {
            if (_sources.Count == 0)
                return null;
            return _sources[_position];
        }

        public void MoveNext()
        {
            if(_position < _sources.Count - 1)
                _position++;
        }

        public void MovePrevious()
        {
            if(_position > 0)
                _position--;
        }
        #endregion
    }
}
