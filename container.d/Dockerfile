FROM library/alpine:latest as builder
LABEL blogger="清雨@https://blog.gazer.win/"
LABEL maintainer="Zerorigin <871670172@qq.com>"

WORKDIR /opt/
RUN sed -i 's/dl-cdn.alpinelinux.org/mirrors.ustc.edu.cn/g' /etc/apk/repositories
RUN set -ex \
    && apk add --no-cache binutils build-base dotnet7-sdk icu-libs git libstdc++ zlib-dev

RUN set -ex \
    mkdir app \
    && git clone --depth 1 https://github.com/zhsy53/Stock2.git \
    && cp -rf ./Stock2/Service/Resources ./app/ \
    && dotnet publish -c Release /p:PublishAot=true --sc --os linux-musl \
    -o "./app/" ./Stock2/Web/Web.csproj \
    && chmod +x ./app/Web \
    && strip ./app/Web


FROM library/alpine:latest
LABEL blogger="清雨@https://blog.gazer.win/"
LABEL maintainer="Zerorigin <871670172@qq.com>"

ENV TZ Asia/Shanghai

WORKDIR /opt/app
COPY --from=builder /opt/app /opt/app


RUN sed -i 's/dl-cdn.alpinelinux.org/mirrors.ustc.edu.cn/g' /etc/apk/repositories
RUN set -ex \
    && apk add --no-cache icu-libs libstdc++ tzdata

RUN set -ex \
    && ln -sf $(echo /usr/share/zoneinfo/${TZ}) /etc/localtime \
    && echo ${TZ} > /etc/timezone

ENTRYPOINT ["./Web"]
