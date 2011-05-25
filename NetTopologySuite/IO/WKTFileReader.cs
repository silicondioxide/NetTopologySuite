using System;
using System.Collections.Generic;
using System.IO;
using GeoAPI.Geometries;

namespace NetTopologySuite.IO
{
    ///<summary>
    /// Reads a sequence of <see cref="IGeometry"/>s in WKT format from a text file.
    ///</summary>
    /// <remarks>The geometries in the file may be separated by any amount of whitespace and newlines.</remarks>
    /// <author>
    /// Martin Davis
    /// </author>
    public class WKTFileReader
    {
        private readonly FileInfo _file;
        private readonly WKTReader _wktReader;
        private int _count;

        ///<summary>
        /// Creates a new <tt>WKTFileReader</tt> given the <tt>File</tt> to read from and a <tt>WKTReader</tt> to use to parse the geometries.
        ///</summary>
        /// <param name="file"> the <tt>FileInfo</tt> to read from</param>
        /// <param name="wktReader">the geometry reader to use</param>
        public WKTFileReader(FileInfo file, WKTReader wktReader)
        {
            _file = file;
            _wktReader = wktReader;
        }

        ///<summary>
        /// Creates a new <tt>WKTFileReader</tt>, given the name of the file to read from.
        ///</summary>
        /// <param name="filename">The name of the file to read from</param>
        /// <param name="wktReader">The geometry reader to use</param>
        public WKTFileReader(String filename, WKTReader wktReader)
            : this(new FileInfo(filename), wktReader)
        {
        }

        ///<summary>
        /// Gets/Sets the maximum number of geometries to read.
        ///</summary>
        public int Limit { get; set; }

        ///<summary>
        /// Gets/Sets the number of geometries to skip before reading.
        ///</summary>
        public int Offset { get; set; }

        ///<summary>
        /// Reads a sequence of geometries.
        ///</summary>
        /// <remarks>
        /// <para>
        /// If an offset is specified, geometries read up to the offset count are skipped.</para>
        /// <para>If a limit is specified, no more than <tt>limit</tt> geometries are read.</para>
        /// </remarks>
        /// <returns>The list of geometries read</returns>
        public IList<IGeometry> Read()
        {
            _count = 0;
            StreamReader fileReader = new StreamReader(_file.OpenRead());
            try
            {
                //BufferedReader bufferedReader = new BufferedReader(fileReader);
                //try {
                return Read(fileReader/*bufferedReader*/);
                //} finally {
                //    bufferedReader.close();
                //}
            }
            finally
            {
                fileReader.Close();
            }
        }

        private IList<IGeometry> Read(/*BufferedReader*/StreamReader bufferedReader)
        {
            IList<IGeometry> geoms = new List<IGeometry>();
            while (!IsAtEndOfFile(bufferedReader) && !IsAtLimit(geoms))
            {
                IGeometry g = _wktReader.Read(bufferedReader);
                if (_count >= Offset)
                    geoms.Add(g);
                _count++;
            }
            return geoms;
        }

        private bool IsAtLimit(IList<IGeometry> geoms)
        {
            if (Limit < 0) return false;
            if (geoms.Count < Limit) return false;
            return true;
        }

        ///<summary>
        /// Tests if reader is at EOF.
        ///</summary>
        private static bool IsAtEndOfFile(/*BufferedReader*/StreamReader bufferedReader)
        {
            return bufferedReader.EndOfStream;
            /*
            bufferedReader.mark(1000);

            StreamTokenizer tokenizer = new StreamTokenizer(bufferedReader);
            int type = tokenizer.nextToken();

            if (type == StreamTokenizer.TT_EOF) {
                return true;
            }
            bufferedReader.reset();
            return false;
             */
        }
    }
}