using System;
using System.Collections.Generic;

namespace Citadel
{
    public abstract class CitadelOptions
    {
        private readonly IList<Configuration.ICitadelExtension> _extensions;

        public CitadelOptions()
        {
            _extensions = new List<Configuration.ICitadelExtension>();
        }

        public IReadOnlyList<Configuration.ICitadelExtension> Extensions => _extensions as IReadOnlyList<Configuration.ICitadelExtension>;


        /// <summary>
        /// Registers an extension that will be executed when building services.
        /// </summary>
        /// <param name="extension"></param>
        public void RegisterExtension(Configuration.ICitadelExtension extension)
        {
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            _extensions.Add(extension);
        }
    }
}
