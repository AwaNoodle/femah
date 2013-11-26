﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Femah.Core
{
    public interface IFeatureSwitchProvider
    {
        /// <summary>
        /// Initialise the provider, given the names of the feature switches.
        /// </summary>
        /// <param name="featureSwitches">Names of the feature switches in the application.</param>
        void Initialise(List<string> featureSwitches);

        /// <summary>
        /// Get a feature switch.
        /// </summary>
        /// <param name="name">The name of the feature switch to get</param>
        /// <returns>An instance of IFeatureSwitch if found, otherwise null</returns>
        IFeatureSwitch Get(string name);

        /// <summary>
        /// Save a feature switch.
        /// </summary>
        /// <param name="featureSwitch">The feature to be saved</param>
        void Save(IFeatureSwitch featureSwitch);

        /// <summary>
        /// Return all feature switches.
        /// </summary>
        /// <returns>A list of zero or more instances of IFeatureSwitch</returns>
        List<IFeatureSwitch> All();
    }
}
