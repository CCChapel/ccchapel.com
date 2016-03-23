using System;

using CMS.OutputFilter;
using CMS.Base;


/// <summary>
/// Sample custom module class. Partial class ensures correct registration. For adding new methods, modify SampleModule inner class.
/// </summary>
[SampleMacroLoader]
public partial class CMSModuleLoader
{
    /// <summary>
    /// Attribute class ensuring correct initialization of custom macro methods and output filter substitutions.
    /// </summary>
    private class SampleMacroLoader : CMSLoaderAttribute
    {
        /// <summary>
        /// Registers module methods.
        /// </summary>
        public override void Init()
        {
            // -- Custom string macro methods
            //Extend<string>.With<CustomMacroMethods>();

            // -- Custom output substitution resolving
            //ResponseOutputFilter.OnResolveSubstitution += OutputFilter_OnResolveSubstitution;
        }


        /// <summary>
        /// Resolves the output substitution
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void OutputFilter_OnResolveSubstitution(object sender, SubstitutionEventArgs e)
        {
            if (!e.Match)
            {
                // Add your custom macro evaluation
                switch (e.Expression.ToLowerCSafe())
                {
                    case "somesubstitution":
                        e.Match = true;
                        e.Result = "Resolved substitution";
                        break;
                }
            }
        }
    }
}