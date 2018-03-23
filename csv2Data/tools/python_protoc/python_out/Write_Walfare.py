# -*- coding: utf-8 -*-
import Walfare_pb2
import codecs

import CommonMoudle

def addItem(item, args):
	item.ID = CommonMoudle.setProtoValuePyExcel(int, args[0], False)
	item.StartTime = CommonMoudle.setProtoValuePyExcel(int, args[1], False)
	item.UserID = CommonMoudle.setProtoValuePyExcel(int, args[2], False)
	item.Type = CommonMoudle.setProtoValuePyExcel(int, args[3], False)
	item.Num = CommonMoudle.setProtoValuePyExcel(int, args[4], False)
	item.Desc = CommonMoudle.setProtoValuePyExcel(str, args[5], False)
	

table = Walfare_pb2.WalfareTable()
table.tname = 'Walfare.bytes'
rf = CommonMoudle.open_excel('txt\\福利_黑名单_公告.xlsx')
sheet = rf.sheet_by_name('Walfare')
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


f = file('bytes\\Walfare.bytes', 'wb')
f.write(table.SerializeToString())
f.close()
