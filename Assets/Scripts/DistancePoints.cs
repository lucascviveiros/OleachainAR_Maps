using System.Collections;
using UnityEngine;
using System;
using Mapbox.Unity.Location;
using UnityEngine.UI;
using System.Globalization;
using Mapbox.Examples;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// DistancePoints class is used for calculating 
/// the distance of nearby olive trees to the current actual user GPS position
/// </summary>
public class DistancePoints : MonoBehaviour
{
	/// Serialized UI Text
	[SerializeField]
	private Text NotificationProviderText;
	[SerializeField]
	private RectTransform rectPanelNotification;
	[SerializeField]
	private Text textDistance;
	[SerializeField]
	private Text textListDistance;
	
	[SerializeField]
	private SpawnOnMap spawnOnMap;

	/// Mapbox location providers
	private AbstractLocationProvider _locationProvider = null;
	private Mapbox.Unity.Location.Location currLoc;

	private SortedList mySL;

	private int index;

	/// Sum of the number of nearby trees
	private int found10_20 = 0;
	private int foundLess10 = 0;
	private int found3_5 = 0;
	private int foundLess3 = 0;
	private int found1_5_3 = 0;
	private int found1_1_5 = 0;
	private int foundNear = 0;

	/// Floats
	//private float xValue = -22.0f;
	private float xValue2 = 44.0f;
	private float timer;
	private float timer2;
	private float timer3;

	/// wait time in seconds
	private float waitTime = 1f; //6
	private float waitTime2 = 2f; //3 
	private float waitTime3 = 1f; //8

	/// Strings
	private string[] myLatLong;
	private string[] myActualLatLongList = new string[2];
	private string found = "";
	private string distance = "";

	/// Bools
	private bool oncePrint = false;
	private bool keepCalculating = true;
	private bool onceNotification = false;

	/// Doubles
	private double distMaisProxima = 1000000;
	private double value;
	private double[] myActualDistanceList = new double[155]; //155

	private double[] myIndiceList = new double[155]; //155
	private double[,] _locationDoubles = new double[155, 2] //155 antes
		{
			{41.802921, -6.774121}, //near home 2
			{41.802901, -6.774131}, //near home
			{41.796782, -6.769779},  //Arvore 1 ipb
			{41.797185, -6.770038}, //Arvore 2 ipb
			{41.802016, -6.768048}, //unidade hospitalar de braganca
			

			{41.014915, -6.956071},	//Freguesia: Escalh�o / Barca d�alva
			{41.014825, -6.956130},
			{41.015108, -6.955879},
			{41.015191, -6.955884},
			{41.015265, -6.955784},
			{41.015287, -6.955612},
			{41.015413, -6.955611},
			{41.015503, -6.955646},
			{41.016026, -6.955386},
			{41.016102, -6.955354},
			{41.016185, -6.955432},
			{41.016367, -6.955308},
			{41.014648, -6.956252},
			{41.014551, -6.956211},
			{41.014433, -6.956271},
			{41.014328, -6.956343},
			{41.014242, -6.956194},
			{41.014209, -6.956288},
			{41.014208, -6.956461},
			{41.014165, -6.956522},
			{41.011997, -7.019437}, //Almendra
			{41.011073, -7.019701},
			{41.011136, -7.019744},
			{41.011109, -7.019878},
			{41.011039, -7.019884},
			{41.011082, -7.020135},
			{41.010945, -7.020159},
			{41.010954, -7.019827},
			{41.010905, -7.019684},
			{41.010948, -7.019534},
			{41.010759, -7.019382},
			{41.010865, -7.019261},
			{41.010898, -7.019172},
			{41.010815, -7.019055},
			{41.010851, -7.018647},
			{41.010771, -7.018578},
			{41.010507, -7.018310},
			{41.010974, -7.019092},
			{41.001603, -7.059390},
			{41.001538, -7.059408},
			{41.001457, -7.059451},
			{41.001322, -7.059253},
			{41.001364, -7.059471},
			{41.001354, -7.059639},
			{41.001348, -7.059816},
			{41.001338, -7.059880},
			{41.001346, -7.059965},
			{41.001462, -7.059657},
			{41.025230, -7.138571}, //Muxagata
			{41.025171, -7.138520},
			{41.025092, -7.138122},
			{41.025021, -7.138323},
			{41.024921, -7.138238},
			{41.024967, -7.138542},
			{41.025072, -7.138611},
			{41.024985, -7.138664},
			{41.024999, -7.138749},
			{41.025005, -7.138855},
			{41.025025, -7.138954},
			{41.025162, -7.138657},
			{41.037742, -7.162662},
			{41.037800, -7.162659},
			{41.037900, -7.162607},
			{41.037803, -7.162427},
			{41.037964, -7.162555},
			{41.038042, -7.162522},
			{41.037898, -7.162400},
			{41.037959, -7.162358},
			{41.038042, -7.162419},
			{41.038127, -7.162473},
			{41.038180, -7.162429},
			{41.038098, -7.162351},
			{41.038095, -7.162254},
			{41.038197, -7.162241},
			{41.038221, -7.162325},
			{41.038250, -7.162152},
			{41.038246, -7.162080},
			{41.038380, -7.161879},
			{41.038437, -7.161829},
			{41.038345, -7.161367},
			{41.074144, -7.132837}, //Salgueiro
			{41.074073, -7.132765},
			{41.074097, -7.132613},
			{41.074032, -7.132664},
			{41.074028, -7.132809},
			{41.073959, -7.132616},
			{41.073997, -7.132548},
			{41.073952, -7.132329},
			{41.073961, -7.132203},
			{41.073875, -7.131955},
			{41.073944, -7.131900},
			{41.074027, -7.131870},
			{41.074007, -7.132022},
			{41.074056, -7.132158},
			{41.074086, -7.132277},
			{41.074107, -7.132397},
			{41.074064, -7.132417},
			{41.073437, -7.132498},
			{41.074209, -7.132654},
			{41.074250, -7.132762},
			{41.103861, -7.140085}, //Vila Nova de Foz C�a	Entrada da costa Foz c�a (Pocinho)
			{41.103799, -7.140085},
			{41.103724, -7.140103},
			{41.103652, -7.140073},
			{41.103590, -7.140095},
			{41.103457, -7.140126},
			{41.103408, -7.140133},
			{41.103340, -7.140174},
			{41.103552, -7.140213},
			{41.103598, -7.140272},
			{41.103678, -7.140259},
			{41.103748, -7.140259},
			{41.103825, -7.140284},
			{41.103827, -7.140372},
			{41.103854, -7.140454},
			{41.103664, -7.140486},
			{41.103681, -7.140541},
			{41.103760, -7.140574},
			{41.103987, -7.140294},
			{41.103917, -7.140118},
			{41.117250, -7.136816}, //Vila Nova de Foz C�a //   Vale Verde, Pocinho
			{41.117323, -7.136775},
			{41.117431, -7.136828},
			{41.117435, -7.136779},
			{41.117740, -7.136708},
			{41.117795, -7.136747},
			{41.117852, -7.136747},
			{41.118000, -7.136837},
			{41.118063, -7.136698},
			{41.118079, -7.136383},
			{41.118206, -7.136143},
			{41.118215, -7.136058},
			{41.117914, -7.136060},
			{41.117795, -7.136029},
			{41.117784, -7.135922},
			{41.117652, -7.136104},
			{41.117653, -7.136244},
			{41.117555, -7.136493},
			{41.117484, -7.136456},
			{41.117361, -7.136349},
			{41.117304, -7.136355},
			{41.117239, -7.136379},
			{41.117302, -7.136477},
			{41.117240, -7.136503},
			{41.117256, -7.136705},
			{41.117434, -7.136676},
			{41.117436, -7.136479},
			{41.117612, -7.136611},
			{41.117565, -7.136557},
			{41.117556, -7.136727} 
		};

	[SerializeField]
	private PopulateList populateList;

	private void Start()
	{
		if (null == _locationProvider)
		{
			_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
		}

		mySL = new SortedList();

		textListDistance.text = "";
		StartCoroutine(waitNotification());
	}

	private void Update()
	{
		currLoc = _locationProvider.CurrentLocation;
		timer += Time.deltaTime;

		if (timer > waitTime) 
		{
			timer -= waitTime; 

			if (keepCalculating == true)
			{
				KeepCalculating();

				keepCalculating = false;
				PrintKeysAndValues(mySL);
				ResetCounters();
			}

			else 
			{
				if (onceNotification == false)
				{
					StartCoroutine(WaitDelatyCalculation());
					//StartCoroutine(ShowNotificationTreeDistance(found, distance));
				}
			}

			///textLocation.text = string.Format("{0}", currLoc.LatitudeLongitude);
			
			//textDistance.text = distance;
			//textDistance.text = distance.Substring(0, 5);

		}

		timer2 += Time.deltaTime;
		while (timer2 >= waitTime2) 
		{
			timer2 -= waitTime2;
			//Debug.Log("distancia : " + distance );

			spawnOnMap.CompareNextTreeLocationsWithSpawnedObjects(_locationDoubles[index, 0], _locationDoubles[index, 1]);

		}

		timer3 += Time.deltaTime;
		while (timer3 >= waitTime3)
		{
			timer3 -= waitTime3;

			double[] myOrderedList = OrderActualDistanceList(myActualDistanceList);

			populateList.Populate(myActualDistanceList.Length, myOrderedList);

			//spawnOnMap.CompareNextTreeLocationsWithSpawnedObjects(_locationDoubles[index - 1, 0], _locationDoubles[index - 1, 1]);
		}

		//textDistance.text = distance.Substring(0, 5);

	}

	private double[] OrderActualDistanceList(double[] distanceList)
    {
		List<double> sortedNumbers = distanceList.OrderBy(number => number).ToList();

		int i = 0;
		foreach (double number in sortedNumbers)
        {
			distanceList[i] = number;
			i++;
        }		

		return distanceList;
	}

	private void ResetCounters()
    {
		found10_20 = 0;
		foundLess10 = 0;
		found3_5 = 0;
		foundLess3 = 0;
		found1_5_3 = 0;
		found1_1_5 = 0;
		
        for (int x = 0; x <= myIndiceList.Length - 1; x++)
        {
			myIndiceList[x]= 0;
		} 
	}

	private IEnumerator WaitForPrint()
    {
		oncePrint = true;
		yield return new WaitForSecondsRealtime(15.0f);
		oncePrint = false;

	}

	public void PrintKeysAndValues(SortedList myList)
	{
		string myResp = "";

		if (oncePrint == false)
        {
			//Debug.Log("distMaisProxima: " + distMaisProxima + " indice: " + index);
			//Debug.Log("Lat: " + _locationDoubles[index - 1, 0] + "|| Long: " + _locationDoubles[index - 1, 1]);
			StartCoroutine(WaitForPrint());
		}

		//Debug.Log("L: " + myList.Count);

		for (int i = 0; i < myList.Count - 1; i++)
		{
			foreach (int line in myActualDistanceList)
			{

				Debug.Log("Line: " + line + " myActualDistanceMatrix: " + myActualDistanceList[line]);
			}

			myResp = myResp + " Key: "+  myList.GetKey(i) + " GetByIndex: "+ myList.GetByIndex(i);
		}

		/*for(int lines = 0; lines <= myIndiceList.Length - 1; lines++)
        {
			Debug.Log("myIndiceList: " + myIndiceList[lines] + " lines: " + lines);
        }*/

		textListDistance.text = myResp;
	}

	private void KeepCalculating()
    {
		SplitCurrentGeolocation();

		for (int i = 0; i <= myActualDistanceList.Length - 1; i++)
		{
			//int sum = myActualDistanceList.Length - 1;
			
			value = myActualDistanceList[i];
			
			if (value != 0 && value <= distMaisProxima)
			{
				distMaisProxima = value;
				index = i;
			}

			if (value == 0) { } //almost impossible

			if (value >= 1 && value <= 1.5)
			{
				foundNear++;
				found1_1_5++;
				found = found1_1_5.ToString() + "[1_1.5]";
				distance = value.ToString();

				if (myIndiceList[0].ToString() == null)
				{
					myIndiceList[0] = value; //new value on list
				}
				else
				{
					if (myIndiceList[0] >= value)
					{
						myIndiceList[0] = value; //new value on list
					}
				}
				string resp = string.Format("Found:{0} . Entre 1 a 1.5 metros de dist�ncia; Value: {1}", found1_1_5, distance);
				Debug.Log("" + resp);

			}

			else if (value >= 1.5 && value <= 2)
			{
				foundNear++;

				found1_5_3++;
				found = found1_5_3.ToString() + "[1.5_2]";
				distance = value.ToString();

				if (myIndiceList[1].ToString() == null)
				{
					myIndiceList[1] = value; //new value on list
				}
                else
                {
					if (myIndiceList[1] >= value)
					{
						myIndiceList[1] = value; //new value on list
					}
				}
				string resp = string.Format("Found:{0} . Entre 1.5 a 2 metros de dist�ncia; Value: {1}", found1_5_3, distance);
				Debug.Log("" + resp);
			}

			else if (value >= 2 && value <= 3)
			{
				foundNear++;

				foundLess3++;
				found = foundLess3.ToString() + "[2_3]";
				distance = value.ToString();

				if (myIndiceList[2].ToString() == null)
				{
					myIndiceList[2] = value; //new value on list
				}
				else
				{
					if (myIndiceList[2] >= value)
					{
						myIndiceList[2] = value; //new value on list
					}
				}
				string resp = string.Format("Found:{0} . Entre 2 a 3 metros de dist�ncia; Value: {1}", foundLess3, distance);
				Debug.Log("" + resp);

			}

			else if (value >= 3 && value <= 5)
			{
				found3_5++;
				found = found3_5.ToString() + "[3_5]";
				distance = value.ToString();

				if (myIndiceList[3].ToString() == null)
				{
					myIndiceList[3] = value; //new value on list
				}
				else
				{
					if (myIndiceList[3] >= value)
					{
						myIndiceList[3] = value; //new value on list
					}
				}
			}

			else if (value >= 5 && value <= 10)
			{
				foundLess10++;
				found = foundLess10.ToString() + "[5_10]";
				distance = value.ToString();

				if (myIndiceList[4].ToString() == null)
				{
					myIndiceList[4] = value; //new value on list
				}
				else
				{
					if (myIndiceList[4] >= value)
					{
						myIndiceList[4] = value; //new value on list
					}
				}
			}

			else if (value >= 10 && value <= 20)
			{
				found10_20++;
				found = found10_20.ToString() + "[10_20]";
				distance = value.ToString();

				if (myIndiceList[5].ToString() == null)
				{
					myIndiceList[5] = value; //new value on list
				}
				else
				{
					if (myIndiceList[5] >= value)
					{
						myIndiceList[5] = value; //new value on list
					}
				}
			}		
		}
	}
	
	private IEnumerator WaitDelatyCalculation()
	{
		//yield return new WaitForSecondsRealtime(11.0f);

		yield return new WaitForSecondsRealtime(1.0f);

		GetCurrentGPSLocation();

		//yield return new WaitForSecondsRealtime(4.0f);
		keepCalculating = true;
		onceNotification = false;

	}


	/*
	private IEnumerator ShowNotificationTreeDistance(string found, string distance)
	{
		yield return new WaitForSecondsRealtime(6.0f);

		if(distMaisProxima == (double.Parse(distance)))
        {
			NotificationProviderText.text = "FOUND:: " + found + " tree near!";
		}
		LeanTween.move(rectPanelNotification, new Vector3(0.0f, xValue, 0f), 1f).setCanvasMove().setDelay(0.2f);

		//yield return new WaitForSecondsRealtime(6.0f); //9 before

		StartCoroutine(waitNotification());

	}*/

	IEnumerator waitNotification()
	{
		LeanTween.move(rectPanelNotification, new Vector3(0.0f, xValue2, 0f), 1f).setCanvasMove(); //Volta notification 
		yield return new WaitForSecondsRealtime(3.0f);

		GetCurrentGPSLocation();

	}

	private void GetCurrentGPSLocation()
	{
		double doubleLat = 0;
		double doubleLong = 0;

		try
		{
			NumberFormatInfo numberFormat = new NumberFormatInfo();
			numberFormat.NumberDecimalSeparator = ".";
			numberFormat.NumberGroupSeparator = ",";

			doubleLat = Convert.ToDouble(SplitCurrentGeolocation()[0], numberFormat);
			doubleLong = Convert.ToDouble(SplitCurrentGeolocation()[1], numberFormat);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}

		CalculateDistanceList(doubleLat, doubleLong);
	}

	private string[] SplitCurrentGeolocation()
	{
		currLoc = _locationProvider.CurrentLocation;
		myLatLong = currLoc.LatitudeLongitude.ToString().Split(',');
		myActualLatLongList[0] = myLatLong[1].ToString();
		myActualLatLongList[1] = myLatLong[0].ToString();

		return myActualLatLongList;
	}

	/// <summary>
	/// Based on my current GPS location. CalculateDistanceList is made with each element of the tree's list previously defined 
	/// </summary>
	/// <param name="userLat1"></param>
	/// <param name="userLong1"></param>
	private void CalculateDistanceList(double userLat1, double userLong1)
	{
		int times = 0;

		for (int i = 0; i <= _locationDoubles.GetLength(0) - 1 ; i++) //GetLength(0) -> Gets the first dimension size
		{
			double myListLat = _locationDoubles[i, 0];
			double myListLong = _locationDoubles[i, 1];
			double theta = userLong1 - myListLong;

			double myDist = Math.Sin(deg2rad(userLat1)) * Math.Sin(deg2rad(myListLat)) + Math.Cos(deg2rad(userLat1)) * Math.Cos(deg2rad(myListLat)) * Math.Cos(deg2rad(theta));
			myDist = Math.Acos(myDist);
			myDist = rad2deg(myDist);

			myDist = myDist * 60 * 1.1515;
			myDist = myDist * 1.609344;
			myDist = myDist / 0.001;
			myDist = Math.Round((Double)myDist, 3); 
			myActualDistanceList[times] = myDist;
			times++;
		}
	}

	public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
	{
		if ((lat1 == lat2) && (lon1 == lon2))
		{
			return 0;
		}
		else
		{
			double theta = lon1 - lon2;
			//Debug.Log("lon1: " + lon1 + "- lon2: " + lon2 + "= theta:" + theta);

			double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
			//Debug.Log("dist: " + dist);
			dist = Math.Acos(dist);
			//dist = rad2deg(dist);

			dist = dist * 60 * 1.1515;
			dist = dist * 1.609344;
			dist = dist / 0.001; //to metro

			Debug.Log("dist init: " + dist);
			return (dist);
		}
	}

	private double deg2rad(double deg)
	{
		return (deg * Math.PI / 180.0);
	}

	private double rad2deg(double rad)
	{
		return (rad / Math.PI * 180.0);
	}

}
