#!/bin/sh


if [ $# -lt 1 ]
then
	echo "Invalid Usage of CopyInstallBuild.sh"
	echo "Usage : CopyBuild.sh <Iphone Device id> pathfolder"
	exit
fi

pathfolder=$2

mountedPath="$2/TestApp/"
username=$3
password=$4
appfilename="Lync.ipa"
buildserverPath=$mountedPath$appfilename
buildlocalPath="$2/Utilities/"
iDeployPath="iDeploy.app/Contents/MacOS/iDeploy"
deviceId=$1

rm -f $buildlocalPath*.ipa

cp $buildserverPath $buildlocalPath 


$buildlocalPath$iDeployPath -u Lync -d $deviceId

echo "sleep 5"

sleep 5

$buildlocalPath$iDeployPath -i $buildlocalPath$appfilename -d $deviceId

