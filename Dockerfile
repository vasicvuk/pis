FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
RUN  apk --update --no-cache add gettext bash
RUN mkdir /app && chmod 777 /app
WORKDIR /app
COPY /out .
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
ENV DB_NAME psd2
CMD ["bash", "/entrypoint.sh"]