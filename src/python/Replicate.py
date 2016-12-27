import json
import os
import sys
from shutil import copy2


def copy_directory_contents(src_dir, dst_dir):
    print src_dir + " ==> " + dst_dir
    file_list = os.listdir(src_dir)
    print "Files in directory: {0}".format(len(file_list))
    if not os.path.isdir(dst_dir):
        os.mkdir(dst_dir)
    for file_name in file_list:
        src_file = src_dir + "\\" + file_name
        if os.path.isfile(src_file):
            print(file_name)
            copy2(src_file, dst_dir)
        else:
            copy_directory_contents(src_file, dst_dir + "\\" + file_name)


if __name__ == '__main__':
    print(sys.argv[1:])
    if len(sys.argv) > 1:
        settings_file_name = sys.argv[1]
    else:
        settings_file_name = "settings.json"
    with open(settings_file_name) as settings_file:
        settings = json.load(settings_file)
    print(settings)

    for path in settings["paths"]:
        copy_directory_contents(path["source"], path["destination"])
