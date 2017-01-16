import json
import os
import sys
from shutil import copy2
import stat
import logging


def copy_directory_contents(src_dir, dst_dir):
    file_list = os.listdir(src_dir)
    if not os.path.isdir(dst_dir):
        os.mkdir(dst_dir)
    for file_name in file_list:
        src_file = src_dir + "\\" + file_name
        dst_file = dst_dir + "\\" + file_name
        if os.path.isfile(src_file):
            if os.access(dst_file, os.F_OK):
                dst_file_time = os.path.getmtime(dst_file)
                src_file_time = os.path.getmtime(src_file)
                if src_file_time > dst_file_time and not isclose(src_file_time, dst_file_time):
                    print("File is newer: {0} > {1}".format(src_file_time, dst_file_time))
                    if not os.access(dst_file, os.W_OK):
                        os.chmod(dst_file, stat.S_IREAD | stat.S_IWRITE)
                    copy_the_file(dst_dir, dst_file, src_file)
            else:
                print("File is new")
                copy_the_file(dst_dir, dst_file, src_file)
        else:
            copy_directory_contents(src_file, dst_file)


def copy_the_file(dst_dir, dst_file, src_file):
    try:
        print(dst_file)
    except:
        pass
    try:
        copy2(src_file, dst_dir)
    except IOError:
        logging.error("Error copying file {0} to {1}".format(src_file, dst_file))


def isclose(a, b, rel_tol=1e-09, abs_tol=0.0):
    return abs(a-b) <= max(rel_tol * max(abs(a), abs(b)), abs_tol)


if __name__ == '__main__':
    logging.basicConfig(format='%(asctime)s:%(levelname)s:%(message)s', filename='replicate.log', level=logging.DEBUG)
    logging.info("Directory replication starting")
    print(sys.argv[1:])
    if len(sys.argv) > 1:
        settings_file_name = sys.argv[1]
    else:
        settings_file_name = "settings.json"
    with open(settings_file_name) as settings_file:
        settings = json.load(settings_file)

    for path in settings["paths"]:
        logging.info("Copying directory contents from %s to %s", path["source"], path["destination"])
        copy_directory_contents(path["source"], path["destination"])

    logging.info("Directory replication finished")