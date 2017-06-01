#!/bin/sh

if [ "$1" == "lang" ]
then
echo "Languages options"
echo "Arabic                        : ar"
echo "English 						: en"
echo "French  						: fr"
echo "Catalan 						: ca"
echo "Czech   						: cs"
echo "Danish  						: da"
echo "German - Austria 				: de"
echo "Greek 						: el"
echo "Spanish - Spain (Traditional) : es"
echo "Finnish 						: fi"
echo "Hebrew                        : he"
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
echo "Usage : RunDeviceTests.sh <Device id> <SuiteName> <language> [<TestCaseName>] [\"deploy\"]"
echo "First Argument is iPad unique device id"
echo "Second Argument is name of test suite to run"
echo "Third Argument is the language on which the build to run"
echo "Fourth Argument (optional) is name of test case in the test suite to run"
echo "Fifth Argument (optional) is fixed string \"deploy\" indicating a fresh deploy of Lync.ipa"
echo "Sixth Argument  is automation path file"
exit
exit
fi

deviceId=$1
suiteName=$2
language=$3

deploy=0;
testCaseName=""
pathFile=""
localName=""

if [ $# -eq 4 ]
then
echo "$4" |grep -q "/"
if [ $? -eq 0 ]
then
pathFile=$4;
else
echo "Invalid Usage of RunTests.sh"
echo "Usage : RunDeviceTests.sh <Device id> <SuiteName> <language> [<TestCaseName>] [\"deploy\"]"
echo "First Argument is iPad unique device id"
echo "Second Argument is name of test suite to run"
echo "Third Argument is the language on which the build to run"
echo "Fourth Argument (optional) is name of test case in the test suite to run"
echo "Fifth Argument (optional) is fixed string \"deploy\" indicating a fresh deploy of Lync.ipa"
echo "Sixth Argument  is automation path file"
exit
fi
fi

if [ $# -eq 5 ]
then
if [ "$4" == "deploy" ]
then
deploy=1
pathFile=$5;
else
testCaseName=$4;
pathFile=$5;
fi
fi

if [ $# -eq 6 ]
then
if [ "$5" == "deploy" ]
then
deploy=1
testCaseName=$4;
pathFile=$6;
fi
fi

echo "$pathFile" |grep -q "/"
if [ $? -eq 0 ]
then
echo "$pathFile"
else
echo "Invalid Usage of RunTests.sh"
echo "Usage : RunDeviceTests.sh <Device id> <SuiteName> <language> [<TestCaseName>] [\"deploy\"]"
echo "First Argument is iPad unique device id"
echo "Second Argument is name of test suite to run"
echo "Third Argument is the language on which the build to run"
echo "Fourth Argument (optional) is name of test case in the test suite to run"
echo "Fifth Argument (optional) is fixed string \"deploy\" indicating a fresh deploy of Lync.ipa"
echo "Sixth Argument  is automation path file"
exit
fi

trace_results_dir=""$pathFile"/Tests/Images"
echo "Current Language is :"$language

if [ $deploy -eq 1 ]
then
chmod +x $pathFile/Utilities/CopyBuild.sh
$pathFile/Utilities/CopyBuild.sh $deviceId $pathFile
cd $pathFile/Tests/
fi


localeArray=("en_US ar_AR he_HE fr_FR ca_ES cs_CZ da_DK de_DE el_GR es_ES fi_FI hr_HR hu_HU id_ID it_IT ja_JP ko_KR ms_MY vi_VN nl_NL nb_NO pl_PL pt_BR pt_PT ro_RO ru_RU sk_SK sv_SE tr_TR uk_UA zh_CN zh_TW")

for i in $localeArray; do
if [[ ("$3" = "pt-PT" && "$i" = "pt_PT") || ("$i" =~ $3) ]] || [[ ("$3" = "zh-Hans" && "$i" = "zh_CN") || ("$3" = "zh-Hant" && "$i" = "zh_TW") ]]; then
localName=$i
chmod +x $pathFile/Utilities/iOSLanguageScript/iOSLanguageChange.py
$pathFile/Utilities/iOSLanguageScript/iOSLanguageChange.py $i
cd $pathFile/Tests/
sleep 95
break;
fi
done


#---------- Copy Localization Resource file start ------------------------------------------------
if [[ ("$3" = "nb") ]]; then
language="no"
fi

localizedStringFile="../resources/"$language".lproj/Localizable.strings"
newResourceFileName="../TestConfigFiles/Localizable.strings"

rm -f $newResourceFileName
if [ -f $localizedStringFile ]
then
$(iconv -f UTF-8 -t UTF-8 $localizedStringFile >> $newResourceFileName)
else
echo $localizedStringFile " file does not exists"
exit
fi

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
instruments -w $deviceId -t /Applications/Xcode.app/Contents/Applications/Instruments.app/Contents/PlugIns/AutomationInstrument.xrplugin/Contents/Resources/Automation.tracetemplate -D instrumentsDriver SfB -e UIASCRIPT UiTests.js -e UIARESULTSPATH $trace_results_dir > $pathFile/Tests/Output.txt

if [ -f ../TestConfigFiles/TestCaseInfo.xml ]
then
echo "Instruments exited before completing Test Run , restarting the run"
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

instruments -w $deviceId -t /Applications/Xcode.app/Contents/Applications/Instruments.app/Contents/PlugIns/AutomationInstrument.xrplugin/Contents/Resources/Automation.tracetemplate -D instrumentsDriver SfB -e UIASCRIPT UiTests.js -e UIARESULTSPATH $trace_results_dir > $pathFile/Tests/Output.txt

if [ -f ../TestConfigFiles/TestCaseInfo.xml ]
then
echo "Instruments exited before completing Test Run , restarting the run"
else
testRunOver=1
fi
done
fi

echo "Test Run Completed"
