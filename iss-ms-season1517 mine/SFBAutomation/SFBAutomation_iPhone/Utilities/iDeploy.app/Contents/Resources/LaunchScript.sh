#!/bin/sh

export IDEPLOY_CUSTOM_URL="${3}"

DEVELOPER_DIRECTORY=`/usr/bin/xcode-select -print-path`

"${DEVELOPER_DIRECTORY}/usr/bin/instruments" -w "${2}" -t "${DEVELOPER_DIRECTORY}/Platforms/iPhoneOS.platform/Developer/Library/Instruments/PlugIns/AutomationInstrument.bundle/Contents/Resources/Automation.tracetemplate" iLaunch -e UIASCRIPT "${1}"
