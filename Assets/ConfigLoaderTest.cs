using NUnit.Framework;
using UnityEngine;
using System;

namespace AssemblyCSharp
{
	[TestFixture()]
	public class ConfigLoaderTest
	{
		[Test()]
		public void TestCase ()
		{
			
		}
		
		[Test()]
		public void ParseVec3Test()
		{
			string[] toParse = new string[]{
			"(1,1,1)",
			 "1,1,1",
			 "1.000,1.000,1.000",
			 "(1.0,1.0,1.0)"};
			
			foreach(string s in toParse){
				Vector3 v = ConfigLoader.ParseVec3(s);
				Assert.That(v == Vector3.one, "'" + s + "' does not result in " + Vector3.one.ToString() + ", but into ->" + v.ToString());
			}
			
		}
		
		
	}
}

