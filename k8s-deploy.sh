#!/bin/bash
set -e

IMAGE_TAG=$1
REPOSITORY_URI=148336247604.dkr.ecr.us-east-1.amazonaws.com/energymicroservice
echo "Updating kubeconfig"
aws eks update-kubeconfig --region us-east-1 --name legacy-cluster1

echo "Updating deployment with new image: $IMAGE_TAG"
kubectl set image deployment/energymicroservice energymicroservice=$REPOSITORY_URI:$IMAGE_TAG

echo "Waiting for rollout to complete"
kubectl rollout status deployment/energymicroservice
