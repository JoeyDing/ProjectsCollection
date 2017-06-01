#!/bin/sh

if [ "$1" == "lang" ]
then
echo "Languages options"
echo "English 						: en"
echo "French  						: fr"
echo "Catalan 						: ca"
echo "Czech   						: cs"
echo "Danish  						: da"
echo "German - Austria 				: de"
echo "Greek 						: el"
echo "Spanish - Spain (Traditional) : es"
echo "Finnish 						: fi"
echo "Croatian 						: hr"
echo "Hungarian 					: hu"
echo "Indonesian 					: id"
echo "Italian - Italy 				: it"
echo "Japanese 						: ja"
echo "Korean 						: ko"
echo "Malay - Malaysia 				: ms"
echo "Dutch - Netherlands 			: nl"
echo "Norwegian - Bokml 			: nb"
echo "Polish 						: pl"
echo "Portuguese - Portugal 		: pt-PT"
echo "Portuguese - Brazil 			: pt"
echo "Romanian - Romania 			: ro"
echo "Russian - Russia 			    : ru"
echo "Slovak 						: sk"
echo "Swedish - Sweden 				: sv"
echo "Turkish 						: tr"
echo "Ukrainian 					: uk"
echo "Chinese - Simplified 			: zh-Hans"
echo "Chinese - Traditional 		: zh-Hant"
exit
fi
if [ $# -lt 2 ]
then
echo "Invalid Usage of RunTests.sh"
echo "Usage : RunSimulatorTests.sh <SuiteName> <language> [<TestCaseName>] [\"deploy\"]"
echo "First Argument is name of test suite to run"
echo "Second Argument is the language on which the build to run"
echo "Third Argument (optional) is name of test case in the test suite to run"
echo "Fourth Argument (optional) is fixed string \"SimulatorInstall\" indicating a fresh deploy of Lync.app"
echo "Fifth Argument  is automation path file"
exit
fi


suiteName=$1
language=$2

deploy=0;
testCaseName="";
pathFile="";

if [ $# -eq 3 ]
then
echo "$3" |grep -q "/"
if [ $? -eq 0 ]
then
pathFile=$3;
else
echo "Invalid Usage of RunTests.sh"
echo "Usage : RunTests.sh <SuiteName> <language> [<TestCaseName>] [\"deploy\"]"
echo "First Argument is name of test suite to run"
echo "Second Argument is the language on which the build to run"
echo "Third Argument (optional) is name of test case in the test suite to run"
echo "Fourth Argument (optional) is fixed string \"SimulatorInstall\" indicating a fresh deploy of Lync.app"
echo "Fifth Argument  is automation path file"
exit
fi
fi

if [ $# -eq 4 ]
then
if [ "$3" == "deploy" ]
then
deploy=1
pathFile=$4;
else
testCaseName=$3;
pathFile=$4;
fi
fi

if [ $# -eq 5 ]
then
if [ "$4" == "deploy" ]
then
deploy=1
testCaseName=$3;
pathFile=$5;
fi
fi

echo "$pathFile" |grep -q "/"
if [ $? -eq 0 ]
then
echo "$pathFile"
else
echo "Invalid Usage of RunTests.sh"
echo "Usage : RunTests.sh <SuiteName> <language> [<TestCaseName>] [\"deploy\"]"
echo "First Argument is name of test suite to run"
echo "Second Argument is the language on which the build to run"
echo "Third Argument (optional) is name of test case in the test suite to run"
echo "Fourth Argument (optional) is fixed string \"SimulatorInstall\" indicating a fresh deploy of Lync.app"
echo "Fifth Argument  is automation path file"
exit
fi

trace_results_dir=""$pathFile"/Tests/Images"

localeArray=("en_US fr_FR ca_ES cs_CZ da_DK de_DE el_GR es_ES fi_FI hr_HR hu_HU id_ID it_IT ja_JP ko_KR ms_MY nl_NL nb_NO pl_PL pt_BR pt_PT ro_RO ru_RU sk_SK sv_SE tr_TR uk_UA zh_CN zh_TW")

for i in $localeArray; do
if [[ ("$2" = "pt-PT" && "$i" = "pt_PT") || ("$i" =~ $2) ]] || [[ ("$2" = "zh-Hans" && "$i" = "zh_CN") || ("$2" = "zh-Hant" && "$i" = "zh_TW") ]]; then
chmod +x $pathFile/Utilities/iOS-Sim-locale
cd $pathFile/Utilities/
./iOS-Sim-locale -sdk 7.1 -language $2 -locale $i
cd $pathFile/Tests/
break;
fi
done

echo "Current Language is :"$language

cd $pathFile/TestApp/
appPath=$(pwd)"/Lync.app"
cd ../Tests/

if [ $deploy -eq 1 ]
then
chmod +x ../Utilities/SimulatorInstall
cd ../Utilities/
./SimulatorInstall launch $appPath --sdk 7.1 --family iphone --quit
cd ../Tests/
fi

if [[ ("$2" = "nb") ]]; then
language="no"
fi

localizedStringFile="../resources/"$language".lproj/Localizable.strings"
newResourceFileName="../TestConfigFiles/Localizable.strings"

rm -f $newResourceFileName
if [ -f $localizedStringFile ]
then
$(iconv -f UTF-16 -t UTF-8 $localizedStringFile >> $newResourceFileName)
else
echo $localizedStringFile " file does not exists"
exit
fi

js_path=$(pwd)"/"UiTests.js

XCODE_PATH=`xcode-select -print-path`
TRACETEMPLATE="$XCODE_PATH/Platforms/iPhoneOS.platform/Developer/Library/Instruments/PlugIns/AutomationInstrument.bundle/Contents/Resources/Automation.tracetemplate"

if [ ! -n "$testCaseName"  ]
then
rm -f ../TestConfigFiles/TestCaseInfo.xml
inputFileName="../TestConfigFiles/"$suiteName".txt"
numberOfTestCases=$([ -s $inputFileName ]  &&  sed -n '$=' $inputFileName  || echo '0')
echo "<TestCaseInfoDetails>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<SuiteName>"$suiteName"</SuiteName>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<NumberOfTestCases>"$numberOfTestCases"</NumberOfTestCases>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<CurrentTestCaseToRun>0</CurrentTestCaseToRun>" >> ../TestConfigFiles/TestCaseInfo.xml
count=0
while read testCase
do
count=$((count+1))
echo "<TestCase"$count">"$testCase"</TestCase"$count">" >> ../TestConfigFiles/TestCaseInfo.xml
done <"$inputFileName"
testRunOver=0
while [ $testRunOver -eq 0 ]
do
instruments -t $TRACETEMPLATE -D instrumentsDriver $appPath -e UIASCRIPT $js_path -e UIARESULTSPATH $trace_results_dir > $pathFile/Tests/Output.txt

if [ -f ../TestConfigFiles/TestCaseInfo.xml ]
then
echo "Instruments exited before completing Test Run , restarting the run"
sleep 5
else
testRunOver=1
fi
done
else

rm -f ../TestConfigFiles/TestCaseInfo.xml
echo "<TestCaseInfoDetails>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<SuiteName>"$suiteName"</SuiteName>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<NumberOfTestCases>1</NumberOfTestCases>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<CurrentTestCaseToRun>0</CurrentTestCaseToRun>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "<TestCase1>"$testCaseName"</TestCase1>" >> ../TestConfigFiles/TestCaseInfo.xml
echo "</TestCaseInfoDetails>" >> ../TestConfigFiles/TestCaseInfo.xml
testRunOver=0
while [ $testRunOver -eq 0 ]
do
instruments -t $TRACETEMPLATE -D instrumentsDriver $appPath -e UIASCRIPT $js_path -e UIARESULTSPATH $trace_results_dir > $pathFile/Tests/Output.txt

if [ -f ../TestConfigFiles/TestCaseInfo.xml ]
then
echo "Instruments exited before completing Test Run , restarting the run"
sleep 5
else
testRunOver=1
fi
done
fi

echo "Test Run Completed"