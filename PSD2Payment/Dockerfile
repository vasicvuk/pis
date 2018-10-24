FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
RUN  apk --update --no-cache add gettext bash
RUN mkdir /app && chmod 777 /app
WORKDIR /app
COPY /out .
ENV DATABASE_NAME psd2
ENV DATABASE_TYPE InMemory
CMD ["dotnet", "PSD2Payment.dll"]