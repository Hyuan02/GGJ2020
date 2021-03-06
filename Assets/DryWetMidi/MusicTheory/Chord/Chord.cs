﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Melanchall.DryWetMidi.Common;

namespace Melanchall.DryWetMidi.MusicTheory
{
    /// <summary>
    /// Represents a chord as a set of notes names.
    /// </summary>
    public sealed class Chord
    {
        #region Constants

        private static readonly Dictionary<ChordQuality, Interval[]> IntervalsByQuality = new Dictionary<ChordQuality, Interval[]>
        {
            [ChordQuality.Major] = new[] { Interval.Get(IntervalQuality.Major, 3), Interval.Get(IntervalQuality.Perfect, 5) },
            [ChordQuality.Minor] = new[] { Interval.Get(IntervalQuality.Minor, 3), Interval.Get(IntervalQuality.Perfect, 5) },
            [ChordQuality.Augmented] = new[] { Interval.Get(IntervalQuality.Major, 3), Interval.Get(IntervalQuality.Augmented, 5) },
            [ChordQuality.Diminished] = new[] { Interval.Get(IntervalQuality.Minor, 3), Interval.Get(IntervalQuality.Diminished, 5) }
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Chord"/> with the specified notes names (for example, C E G).
        /// </summary>
        /// <param name="notesNames">The set of notes names.</param>
        /// <exception cref="ArgumentNullException"><paramref name="notesNames"/> is null.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="notesNames"/> contains an invalid value.</exception>
        /// <exception cref="ArgumentException"><paramref name="notesNames"/> is empty collection.</exception>
        public Chord(ICollection<NoteName> notesNames)
        {
            ThrowIfArgument.IsNull(nameof(notesNames), notesNames);
            ThrowIfArgument.ContainsInvalidEnumValue(nameof(notesNames), notesNames);
            ThrowIfArgument.IsEmptyCollection(nameof(notesNames), notesNames, "Notes names collection is empty.");

            NotesNames = notesNames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chord"/> with the specified root note name and
        /// names of the notes above root one (for example, C and E G).
        /// </summary>
        /// <param name="rootNoteName">The root note's name.</param>
        /// <param name="notesNamesAboveRoot">The set of names of the notes above root one.</param>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="rootNoteName"/> specified an invalid value. -or-
        /// <paramref name="notesNamesAboveRoot"/> contains an invalid value.</exception>
        public Chord(NoteName rootNoteName, params NoteName[] notesNamesAboveRoot)
            : this(new[] { rootNoteName }.Concat(notesNamesAboveRoot ?? Enumerable.Empty<NoteName>()).ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chord"/> with the specified root note name and
        /// intervals from root one.
        /// </summary>
        /// <param name="rootNoteName">The root note's name.</param>
        /// <param name="intervalsFromRoot">Intervals from root note.</param>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="rootNoteName"/> specified an invalid value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="intervalsFromRoot"/> is null.</exception>
        public Chord(NoteName rootNoteName, IEnumerable<Interval> intervalsFromRoot)
        {
            ThrowIfArgument.IsInvalidEnumValue(nameof(rootNoteName), rootNoteName);
            ThrowIfArgument.IsNull(nameof(intervalsFromRoot), intervalsFromRoot);

            NotesNames = new[] { Interval.Zero }.Concat(intervalsFromRoot)
                .Where(i => i != null)
                .OrderBy(i => i.HalfSteps)
                .Select(i => rootNoteName.Transpose(i))
                .ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chord"/> with the specified root note name and
        /// intervals from root one.
        /// </summary>
        /// <param name="rootNoteName">The root note's name.</param>
        /// <param name="intervalsFromRoot">Intervals from root note.</param>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="rootNoteName"/> specified an invalid value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="intervalsFromRoot"/> is null.</exception>
        public Chord(NoteName rootNoteName, params Interval[] intervalsFromRoot)
            : this(rootNoteName, intervalsFromRoot as IEnumerable<Interval>)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the chord's notes names.
        /// </summary>
        public ICollection<NoteName> NotesNames { get; }

        /// <summary>
        /// Gets the root note's name of the chord.
        /// </summary>
        public NoteName RootNoteName => NotesNames.First();

        #endregion

        #region Methods

        /// <summary>
        /// Gets the collection of chord's inversions.
        /// </summary>
        /// <returns>Collection of chord's inversions.</returns>
        public IEnumerable<Chord> GetInversions()
        {
            var notesNames = new Queue<NoteName>(NotesNames);

            for (var i = 1; i < NotesNames.Count; i++)
            {
                var noteName = notesNames.Dequeue();
                notesNames.Enqueue(noteName);

                yield return new Chord(notesNames.ToArray());
            }
        }

        /// <summary>
        /// Converts the string representation of a musical chord to its <see cref="Chord"/> equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string containing a chord to convert.</param>
        /// <param name="chord">When this method returns, contains the <see cref="Chord"/>
        /// equivalent of the musical chord contained in <paramref name="input"/>, if the conversion succeeded,
        /// or null if the conversion failed. The conversion fails if the <paramref name="input"/> is null or
        /// <see cref="string.Empty"/>, or is not of the correct format. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.</param>
        /// <returns>true if <paramref name="input"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string input, out Chord chord)
        {
            return ParsingUtilities.TryParse(input, ChordParser.TryParse, out chord);
        }

        /// <summary>
        /// Converts the string representation of a musical chord to its <see cref="Chord"/> equivalent.
        /// </summary>
        /// <param name="input">A string containing a chord to convert.</param>
        /// <returns>A <see cref="Chord"/> equivalent to the musical chord contained in <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="input"/> is null or contains white-spaces only.</exception>
        /// <exception cref="FormatException"><paramref name="input"/> has invalid format.</exception>
        public static Chord Parse(string input)
        {
            return ParsingUtilities.Parse<Chord>(input, ChordParser.TryParse);
        }

        /// <summary>
        /// Creates an instance of the <see cref="Chord"/> as triad with possible addition of another notes by intervals
        /// from the root one.
        /// </summary>
        /// <param name="rootNoteName">The root note's name.</param>
        /// <param name="chordQuality">Chord's quality.</param>
        /// <param name="intervalsFromRoot">Intervals from root note.</param>
        /// <returns>Chord created by triad with addition of notes defined by <paramref name="intervalsFromRoot"/>.</returns>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="rootNoteName"/> specified an invalid value. -or-
        /// <paramref name="chordQuality"/> specified an invalid value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="intervalsFromRoot"/> is null.</exception>
        public static Chord GetByTriad(NoteName rootNoteName, ChordQuality chordQuality, params Interval[] intervalsFromRoot)
        {
            ThrowIfArgument.IsInvalidEnumValue(nameof(rootNoteName), rootNoteName);
            ThrowIfArgument.IsInvalidEnumValue(nameof(chordQuality), chordQuality);
            ThrowIfArgument.IsNull(nameof(intervalsFromRoot), intervalsFromRoot);

            var intervals = IntervalsByQuality[chordQuality];
            return new Chord(rootNoteName, intervals.Concat(intervalsFromRoot));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if two <see cref="Chord"/> objects are equal.
        /// </summary>
        /// <param name="chord1">The first <see cref="Chord"/> to compare.</param>
        /// <param name="chord2">The second <see cref="Chord"/> to compare.</param>
        /// <returns>true if the chords are equal, false otherwise.</returns>
        public static bool operator ==(Chord chord1, Chord chord2)
        {
            if (ReferenceEquals(chord1, chord2))
                return true;

            if (ReferenceEquals(null, chord1) || ReferenceEquals(null, chord2))
                return false;

            return chord1.NotesNames.SequenceEqual(chord2.NotesNames);
        }

        /// <summary>
        /// Determines if two <see cref="Chord"/> objects are not equal.
        /// </summary>
        /// <param name="chord1">The first <see cref="Chord"/> to compare.</param>
        /// <param name="chord2">The second <see cref="Chord"/> to compare.</param>
        /// <returns>false if the chords are equal, true otherwise.</returns>
        public static bool operator !=(Chord chord1, Chord chord2)
        {
            return !(chord1 == chord2);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Join(" ", NotesNames.Select(n => n.ToString().Replace(Note.SharpLongString, Note.SharpShortString)));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this == (obj as Chord);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 17;

                foreach (var note in NotesNames)
                {
                    result = result * 23 + note.GetHashCode();
                }

                return result;
            }
        }

        #endregion
    }
}
