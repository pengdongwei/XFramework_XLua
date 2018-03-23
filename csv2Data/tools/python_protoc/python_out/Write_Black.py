# -*- coding: utf-8 -*-
import Black_pb2
import codecs

import CommonMoudle

def addItem(item, args):
	item.ID = CommonMoudle.setProtoValuePyExcel(int, args[0], False)
	item.UseID = CommonMoudle.setProtoValuePyExcel(int, args[1], False)
	item.Name = CommonMoudle.setProtoValuePyExcel(str, args[2], False)
	item.EndTime = CommonMoudle.setProtoValuePyExcel(int, args[3], False)
	item.Type = CommonMoudle.setProtoValuePyExcel(int, args[4], False)
	

table = Black_pb2.BlackTable()
table.tname = 'Black.bytes'
rf = CommonMoudle.open_excel('txt\\福利_黑名单_公告.xlsx')
sheet = rf.sheet_by_name('Black')
rows = sheet.nrows
start_row = 3


for i in range(start_row, rows):
    line = sheet.row_values(i)
    if (len(line) == 0):break
    if (line[0] == ''):break
    try:
        addItem(table.tlist.add(), line)
    except Exception,e:
        print e
        print 'addItem Erorr!!! at line %d , table: %s' % (i, table.tname)
        break


f = file('bytes\\Black.bytes', 'wb')
f.write(table.SerializeToString())
f.close()
