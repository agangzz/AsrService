docker build -t aspnetapp . && docker run -v D:/testdotnetfordocker/testdotnetfordocker/Tools/cvte:/usr/local/kaldi/egs/cvte -t -i --entrypoint /bin/bash aspnetapp:latest 