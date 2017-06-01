#! /usr/bin/python

import os
import sys
import re
import subprocess
import binascii
import hashlib
import shutil
import time

#return the string to be searched for within the mbdb file
def getsearchstring(domain, file, file_length):
	domain_hex = binascii.hexlify(domain)
	file_hex = binascii.hexlify(file)
	file_length_hex = "{0:0{1}x}".format(file_length,4)

	return domain_hex+file_length_hex+file_hex

#return the UUID of the connected iOS device (first device connected if multiple are connected)
def getserial():
	output = subprocess.Popen(['ioreg', '-Src', 'IOUSBDevice'], stdout=subprocess.PIPE).communicate()[0]
	serial_regex = re.compile('USB Serial Number\" = \"(\w{40})\"')
	serial = serial_regex.search(output).group(1)
	print serial
	return serial

#gets the length of the next word to be read
def getlength(numbytes):
	if numbytes == 'ffff':
		return 0
	return int(numbytes,16)*2

#returns the SHA-1 hash of the data
def getsha1(file):
	with open(file, 'rb') as f:
		content = f.read()
	h = hashlib.sha1()
	h.update(content)
	return h.hexdigest()
	
#creates an archive of the old mbdb and .plist
def makearchive(UUID, root_directory):
	date = time.strftime("%Y%m%d", time.gmtime())
	directory = root_directory+UUID+'-'+date+'-'+str(int(time.time()))
	print 'archiving old files to '+directory
	if not os.path.exists(directory):
		os.makedirs(directory)
	return directory
	
#saves the old backed-up .plist to the archive location
def archive_plist(plist_file, UUID, root_directory):
	directory = makearchive(UUID,root_directory)
	shutil.move(plist_file,directory)

	return directory
	
#saves the .mbdb to the archive location
def archive_mbdb(mbdb_file, directory):
	shutil.move(mbdb_file,directory)

#writes the new .mbdb into the file
def write_mbdb(post_processed_data):
	new_mbdb = open(mbdb_file, 'w+')
	new_mbdb.write(post_processed_data)
	new_mbdb.close()
	
#calls the AppleMobileBackup process to initiate a backup of the connected USB iOS device
def restoredevice(UUID):
	subprocess.call(['/System/Library/PrivateFrameworks/MobileDevice.framework/Versions/A/AppleMobileDeviceHelper.app/Contents/Resources/AppleMobileBackup','-r','-s',UUID])

#constants
script_dir = os.path.dirname(os.path.realpath(__file__))
archive_dir = script_dir+'/Archive/'
UUID = getserial()
backup_dir = '/Users/Ning/Library/Application Support/MobileSync/Backup/'+UUID+'/'
mbdb_file = 'Manifest.mbdb'
domain = 'HomeDomain'
file = 'Library/Preferences/.GlobalPreferences.plist'
plist_file = '0dc926a1810f7aee4e8f38793ed788701f93bf9d'
new_mbdb = 'Manifest_new.mbdb'

#get language or exit if the language is blank
if len(sys.argv) == 2:
	language_file = script_dir+'/SettingsFiles/'+sys.argv[1]
	print language_file
else:
	sys.exit()

#change script working directory to the backup directory of connected USB device
os.chdir(backup_dir);

file_length = len(file)

#generate the string to be searched for
search_string = getsearchstring(domain, file, file_length)

#read the contents of the file
with open(mbdb_file, 'rb') as f:
    content = f.read()

#convert the .mbdb file to hex for processing
content_hex = binascii.hexlify(content)

#go to the place with the matching domain and file combination
index = content_hex.find(search_string)+len(search_string)

#skip over the "linked targets"
index+=getlength(content_hex[index:index+4])+4

#get the indexes for the SHA-1 Hash
start_index=index+4
end_index=start_index+getlength(content_hex[index:index+4])

new_language_file = backup_dir+plist_file
#new_language_file)

#archive the old plist
archive_dir = archive_plist(plist_file, UUID, archive_dir)

#copy over settings .plist
shutil.copy2(language_file, new_language_file);

#generate the new mbdb
new_hash = getsha1(new_language_file)

#concatenate the new hash with the rest of the file
post_processed = content_hex[:start_index]+new_hash+content_hex[end_index:]

#convert hex format back to binary
post_processed_data = binascii.unhexlify(post_processed)

#archive old mbdb
archive_mbdb(mbdb_file, archive_dir)

#write new file
write_mbdb(post_processed_data)

#restoring the device based on the UUID
restoredevice(UUID)

next