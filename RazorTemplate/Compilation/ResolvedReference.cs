namespace Templates
{
    public class ResolvedReference
    {
        public ResolvedReference(string name, string resolvedPath)
        {
            Name = name;
            ResolvedPath = resolvedPath;
        }

        /// <summary>
        /// Full path of the referenced assembly.
        /// </summary>
        public string ResolvedPath { get; }

        /// <summary>
        /// Name of the referenced assembly.
        /// </summary>
        public string Name { get; }
    }
}
