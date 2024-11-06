@echo off
REM 获取当前目录
set "baseDirectory=%cd%"

REM 查找并删除所有子目录中的 .tmp 文件
for /R "%baseDirectory%" %%f in (*.tmp) do (
    echo Deleting %%f
    del "%%f"
)

echo All .tmp files in subdirectories of %baseDirectory% have been deleted.
pause