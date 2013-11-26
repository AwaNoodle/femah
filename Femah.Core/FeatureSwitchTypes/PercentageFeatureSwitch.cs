﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Femah.Core.FeatureSwitchTypes
{
    /// <summary>
    /// A simple feature switch that is on for a set percentage of users.
    /// The state of the switch is persisted in the user's cookies.
    /// If no cookie exists the state is chosen at random (weighted according to the percentage), 
    /// and then stored in a cookie.
    /// </summary>
    public class PercentageFeatureSwitch : IFeatureSwitch
    {
        private Random _random;
        private Func<double> _randomGenerator = null;

        /// <summary>
        /// The percentage of users who should see this feature.
        /// Eg. 10 means the feature is on for 10% of users.
        /// </summary>
        public int PercentageOn { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PercentageFeatureSwitch()
        {
            _random = new Random();
            _randomGenerator = _random.NextDouble;
        }

        /// <summary>
        /// Initialise a PercentageFeatureSwitch with a custom random-number generator function.
        /// </summary>
        /// <param name="randomGenerator">A function which returns a random value between 0.0 and 1.0 on each call.</param>
        public PercentageFeatureSwitch(Func<double> randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        #region IFeatureSwitch members

        public bool IsEnabled {get;set;}

        public string Name { get;set;}

        public string FeatureType { get; set; }

        public bool IsOn(IFemahContext context)
        {
            bool isOn;

            if ( context.HttpContext == null || context.HttpContext.Request == null )
            {
                return false;
            }

            // Check if a cookie for this switch exists already.
            if ( context.HttpContext.Request.Cookies.AllKeys.Contains(this.Name) )
            {
                if (!Boolean.TryParse(context.HttpContext.Request.Cookies[this.Name].Value, out isOn))
                {
                    isOn = false;
                }
            }
            else
            {
                // No cookie set.  Choose randomly if feature should be set or not.
                double threshold = PercentageOn / 100.0;
                isOn = _randomGenerator() < threshold;

                // Save value to cookie.
                context.HttpContext.Response.Cookies.Add( new HttpCookie(this.Name, isOn.ToString()) );
            }

            return isOn;
        }

        public void RenderUI(HtmlTextWriter writer)
        {
            return;
        }

        public void SetCustomAttributes(NameValueCollection values)
        {
            string percentage = values["percentage"];
            int percentageValue;
            if (!String.IsNullOrWhiteSpace(percentage) && int.TryParse(percentage, out percentageValue) )
            {
                this.PercentageOn = percentageValue;
            }
        }

        #endregion

    }
}
