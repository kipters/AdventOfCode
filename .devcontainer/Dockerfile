FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim
ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=${USER_UID}

RUN apt-get update && apt-get -y install --no-install-recommends apt-utils dialog 2>&1 \
    && apt-get -y install openssh-client less iproute2 apt-transport-https gnupg2 curl lsb-release \
    git procps ca-certificates python3-pip vim nano groff zip file jq wget \
    # Create a non-root user
    && groupadd --gid ${USER_GID} ${USERNAME} \
    && useradd -s /bin/bash --uid ${USER_UID} --gid ${USER_GID} -m ${USERNAME} \
    # add sudo
    && apt-get install -y sudo \
    && echo ${USERNAME} ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/${USERNAME} \
    && chmod 0440 /etc/sudoers.d/${USERNAME}

# Cleanup APT
RUN apt-get autoremove -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*

USER ${USERNAME}

ENV PATH="${PATH}:/home/${USERNAME}/.dotnet/tools:/home/${USERNAME}/.local/bin"

ENV DEBIAN_FRONTEND=dialog

WORKDIR /workspace
